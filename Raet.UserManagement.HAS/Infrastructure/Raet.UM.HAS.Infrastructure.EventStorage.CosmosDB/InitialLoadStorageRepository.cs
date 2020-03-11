using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using Raet.UM.HAS.Core.Application;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Crosscutting.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Infrastructure.Storage.CosmosDB
{
    public class InitialLoadStorageRepository : CosmosDBRepository, IWriteRawEventStorage
    {
        public InitialLoadStorageRepository(ICosmoDBStorageInitializer clientInitializer, ILogger logger)
        {
            _client = clientInitializer?.Initialize() ?? throw new ArgumentNullException(nameof(clientInitializer));
            _dbConfig = clientInitializer?.DbConfig ?? throw new ArgumentNullException(nameof(clientInitializer.DbConfig));
            _logger = logger;
        }

        public async Task<string> WriteRawEventAsync(EffectiveAuthorizationEvent effectiveAuthorizationEvent)
        {
            var effectiveAuthorizationEventModel = DomainAdapter.MapDomainToWriteStorageModel(effectiveAuthorizationEvent);
            try
            {
                var resourceResponse = await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_dbConfig.Database, _dbConfig.Collection), effectiveAuthorizationEventModel);
                _logger.LogInformation("Data is Inserted");
                return resourceResponse.Resource.Id;
            }
            catch (Exception e)
            {

                throw new RawEventStorageException($"Storage failure causes process interruption due to :{e.Message}", e);
            }
        }
    }
}
