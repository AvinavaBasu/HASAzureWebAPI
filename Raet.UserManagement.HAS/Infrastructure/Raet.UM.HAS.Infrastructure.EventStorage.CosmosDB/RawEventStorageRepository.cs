using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using Raet.UM.HAS.Core.Application;
using Raet.UM.HAS.Core.Reporting;
using Raet.UM.HAS.Crosscutting.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raet.UM.HAS.Core.Reporting.Interface;
using Domain = Raet.UM.HAS.Core.Domain;

namespace Raet.UM.HAS.Infrastructure.Storage.CosmosDB
{
    public class RawEventStorageRepository : CosmosDBRepository, IWriteRawEventStorage, IReadRawEventStorage, IDisposable
    {
        /// <summary>
        /// Constructor to be used by WebAPI with Tracing.NetCore package 
        /// ILoggerFactory injection
        /// </summary>
        /// <param name="clientInitializer"></param>
        /// <param name="factory"></param>
        public RawEventStorageRepository(ICosmoDBStorageInitializer clientInitializer, ILoggerFactory factory)
        {
            _client = clientInitializer?.Initialize() ?? throw new ArgumentNullException(nameof(clientInitializer));
            _dbConfig = clientInitializer?.DbConfig ?? throw new ArgumentNullException(nameof(clientInitializer.DbConfig));
            _logger = factory.CreateLogger("CosmosDbRawEventStorage");
        }

        /// <summary>
        /// Constructor to be used for Azure Function
        /// Ilogger Injection
        /// </summary>
        /// <param name="clientInitializer"></param>
        /// <param name="logger">Azure function injects Ilogger</param>
        public RawEventStorageRepository(ICosmoDBStorageInitializer clientInitializer, ILogger logger)
        {
            _client = clientInitializer?.Initialize() ?? throw new ArgumentNullException(nameof(clientInitializer));
            _dbConfig = clientInitializer?.DbConfig ?? throw new ArgumentNullException(nameof(clientInitializer.DbConfig));
            _logger = logger;
        }

                             
        public async Task<string> WriteRawEventAsync(Domain.EffectiveAuthorizationEvent effectiveAuthorizationEvent)
        {   
            var effectiveAuthorizationEventModel = DomainAdapter.MapDomainToWriteStorageModel(effectiveAuthorizationEvent);
            try
            {
                var resourceResponse = await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_dbConfig.Database, _dbConfig.Collection), effectiveAuthorizationEventModel);
                return resourceResponse.Resource.Id;
            }
            catch (Exception e) {

               throw new RawEventStorageException($"Storage failure causes process interruption due to :{e.Message}",e);
            }
        }

        public Task<List<Domain.EffectiveAuthorizationEvent>> GetRawEventsAsync(
            Domain.EffectiveAuthorization effectiveAuthorization)
        {
            try
            {
                //The reason for composing the full SQL statement instead of using the Equals operation as part of a linq "where" is because "Equals" operator is not supported
                var resourceResponse = _client.CreateDocumentQuery<Models.ReadEffectiveAuthorizationEvent>(
                    UriFactory.CreateDocumentCollectionUri(_dbConfig.Database, _dbConfig.Collection),
                    GetSQLQueryByEffectiveAuthorisation(effectiveAuthorization), _queryOptions).ToList();

                // can't filter event with null target in cosmos db, so doing the event with target null filter here.
                if (effectiveAuthorization.Target is null)
                {
                    resourceResponse = resourceResponse.Where(e => e.EffectiveAuthorization.Target is null).ToList();
                }

                var output = new List<Domain.EffectiveAuthorizationEvent>();
                foreach (Models.ReadEffectiveAuthorizationEvent readModelEvent in resourceResponse)
                {
                    output.Add(DomainAdapter.MapReadStorageModelToDomain(readModelEvent));
                }

                return Task.FromResult<List<Domain.EffectiveAuthorizationEvent>>(output);
            }
            catch (RawEventStorageException ex)
            {
                throw new RawEventStorageException($"Issue while reading events from the storage account :{ex.Message}",
                    ex);
            }
        }

        private string GetSQLQueryByEffectiveAuthorisation(Domain.EffectiveAuthorization ea)
        {
            var queryBuilder = new List<string>();

            queryBuilder.Add($@"c.EffectiveAuthorization.TenantId = ""{ea.TenantId}""");

            queryBuilder.Add($@"c.EffectiveAuthorization.User.Context = ""{ea.User.Context}""");
            queryBuilder.Add($@"c.EffectiveAuthorization.User.Id = ""{ea.User.Id}""");

            queryBuilder.Add($@"c.EffectiveAuthorization.Permission.Application = ""{ea.Permission.Application}""");
            queryBuilder.Add($@"c.EffectiveAuthorization.Permission.Id = ""{ea.Permission.Id}""");

            if (ea.Target != null)
            {
                queryBuilder.Add($@"c.EffectiveAuthorization.Target.Context = ""{ea.Target.Context}""");
                queryBuilder.Add($@"c.EffectiveAuthorization.Target.Id = ""{ea.Target.Id}""");
            }

            var whereClause = String.Join(" AND ", queryBuilder.ToArray());
            return $"SELECT * FROM c WHERE {whereClause}";
        }

        public async Task DeleteRawEventAsync(string documentId, RequestOptions requestOptions)
        {
            await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_dbConfig.Database, _dbConfig.Collection, documentId), requestOptions);
        }

        public void Dispose()
        {
           _client.Dispose();
        }
    }
}
