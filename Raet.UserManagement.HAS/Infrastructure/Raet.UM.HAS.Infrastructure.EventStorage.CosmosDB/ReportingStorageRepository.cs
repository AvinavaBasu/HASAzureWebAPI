using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Crosscutting.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Infrastructure.Storage.CosmosDB
{
    public class ReportingStorageRepository : CosmosDBRepository, IReportingStorage, IDisposable
    {
        public ReportingStorageRepository(ICosmoDBStorageInitializer clientInitializer, ILogger logger)
        {
            _client = clientInitializer?.Initialize() ?? throw new ArgumentNullException(nameof(clientInitializer));
            _dbConfig = clientInitializer?.DbConfig ?? throw new ArgumentNullException(nameof(clientInitializer.DbConfig));
            _logger = logger;
        }

        private string GetSQLQueryByEffectiveAuthorisation(EffectiveAuthorization ea)
        {
            var queryBuilder = new List<string>();

            queryBuilder.Add($@"c.TenantId = ""{ea.TenantId}""");

            queryBuilder.Add($@"c.User.Key.Context = ""{ea.User.Context}""");
            queryBuilder.Add($@"c.User.Key.Id = ""{ea.User.Id}""");

            queryBuilder.Add($@"c.Permission.Application = ""{ea.Permission.Application}""");
            queryBuilder.Add($@"c.Permission.Id = ""{ea.Permission.Id}""");

            if (ea.Target != null)
            {
                queryBuilder.Add($@"c.TargetPerson.Key.Context = ""{ea.Target.Context}""");
                queryBuilder.Add($@"c.TargetPerson.Key.Id = ""{ea.Target.Id}""");
            }

            var whereClause = String.Join(" AND ", queryBuilder.ToArray());
            return $"SELECT * FROM c WHERE {whereClause}";
        }

        private string GetSQLQueryForReporting(ReportingEvent reportingEvent)
        {
            string startDate = string.Format("{0:s}", reportingEvent.StartDate.AddDays(-1).ToUniversalTime());

            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM c WHERE");
            queryBuilder.Append("((c.EffectiveInterval.Start between '"+ startDate + "' AND '"+reportingEvent.EndDateFormat+"') OR ");
            queryBuilder.Append("(c.EffectiveInterval.Start < '"+ reportingEvent.StartDateFormat + "' AND (IS_NULL(c.EffectiveInterval['End']) OR  c.EffectiveInterval['End'] >='"+ reportingEvent.StartDateFormat + "'))");
            queryBuilder.Append(")");
            queryBuilder.Append(" AND c.Permission.Application = '"+ reportingEvent.Application + "'");
            if (!string.IsNullOrWhiteSpace(reportingEvent.Target.Context))
            {
                queryBuilder.Append($@"AND c.TargetPerson.Key.Context = ""{reportingEvent.Target.Context}""");
                queryBuilder.Append($@"AND c.TargetPerson.Key.Id = ""{reportingEvent.Target.Id}""");
            }
            else if(!string.IsNullOrWhiteSpace(reportingEvent.Source.Context))
            {
                queryBuilder.Append($@"AND c.User.Key.Context = ""{reportingEvent.Source.Context}""");
                queryBuilder.Append($@"AND c.User.Key.Id = ""{reportingEvent.Source.Id}""");
            }
            queryBuilder.Append($@"AND c.TenantId = ""{reportingEvent.TenantId}""");
            queryBuilder.Append($@"AND c.Permission.Id IN ({ "\"" + string.Join("\", \"", reportingEvent.Permissions) + "\""})");
            return queryBuilder.ToString();
        }

        public Task<IList<EffectiveAuthorizationInterval>> GetIntervals(EffectiveAuthorization effectiveAuthorization)
        {
            //The reason for composing the full SQL statement instead of using the Equals operation as part of a linq "where" is because "Equals" operator is not supported
            var resourceResponse = _client.CreateDocumentQuery<EffectiveAuthorizationInterval>(UriFactory.CreateDocumentCollectionUri(_dbConfig.Database, _dbConfig.Collection), GetSQLQueryByEffectiveAuthorisation(effectiveAuthorization), _queryOptions).ToList();

            return Task.FromResult<IList<EffectiveAuthorizationInterval>>(resourceResponse);
        }

        public async Task<IList<EffectiveAuthorizationInterval>> FetchReportingData(ReportingEvent ReportingEvent)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(_dbConfig.Database, _dbConfig.Collection);
            var reportingSql = GetSQLQueryForReporting(ReportingEvent);
            var resourceResponse = _client.CreateDocumentQuery<EffectiveAuthorizationInterval>(uri, reportingSql, _queryOptions).ToList();
            return resourceResponse;
        }

        public async Task<string> SaveAsync(IList<EffectiveAuthorizationInterval> effectiveAuthorizationIntervals)
        {
            //FIXME this code is a fudge to be kept until a decision is made about the effectiveAuthorizationIntervals saving individually or in collection
            string[] output = new string[effectiveAuthorizationIntervals.Count];

            try
            {
                //FIXME because there is no way to identify the document in DB matching the Domain entity, the Upsert will always insert. That's why previous deletion is necessary.
                //FIXME some decision needs to be taken about this.

                var effectiveAuthorization = new EffectiveAuthorization
                {
                    TenantId = effectiveAuthorizationIntervals[0].TenantId,
                    User = effectiveAuthorizationIntervals[0].User.Key,
                    Permission = effectiveAuthorizationIntervals[0].Permission,
                    Target = effectiveAuthorizationIntervals[0].TargetPerson?.Key ?? null
                };

                await DeleteIntervals(effectiveAuthorization);

                var resourceResponses = new List<ResourceResponse<Document>>();
                foreach (var t in effectiveAuthorizationIntervals)
                {
                    var response = _client.CreateDocumentAsync(
                        UriFactory.CreateDocumentCollectionUri(_dbConfig.Database, _dbConfig.Collection),
                        t).Result;
                    resourceResponses.Add(response);
                }
                return string.Join(",", resourceResponses.Select(x => x.Resource.Id));
            }
            catch (Exception e)
            {

                _logger.LogCritical($"Storage failure causes process interruption due to :{e}");
                throw new ReportingStorageException(e.Message, e);
            }
        }

        public async Task DeleteIntervals(EffectiveAuthorization effectiveAuthorization)
        {
            try
            {
                var resourceResponse = _client
                    .CreateDocumentQuery<Document>(
                        UriFactory.CreateDocumentCollectionUri(_dbConfig.Database, _dbConfig.Collection),
                        GetSQLQueryByEffectiveAuthorisation(effectiveAuthorization), _queryOptions).ToList();
                var tasks = new List<Task>();
                var requestOptions = new RequestOptions
                {
                    PartitionKey = new PartitionKey(effectiveAuthorization.TenantId)
                };
                foreach (Document doc in resourceResponse)
                {
                    tasks.Add(_client.DeleteDocumentAsync(doc.SelfLink, requestOptions));
                }

                Task.WaitAll(tasks.ToArray());
            }
            catch (Exception e)
            {
                _logger.LogCritical($"Storage failure causes process interruption due to :{e}");
                throw new ReportingStorageException(e.Message, e);
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public Task<IList<Core.Domain.Permission>> FetchPermissionData(string application, string tenantId)
        {
            var permissions = _client.CreateDocumentQuery<dynamic>(UriFactory.CreateDocumentCollectionUri(_dbConfig.Database, _dbConfig.Collection), GetSQLQueryForPermission(application,tenantId), _queryOptions).ToList();
            IList<Core.Domain.Permission> result = permissions.Select(e => new Core.Domain.Permission
            {
                Application = (string)e.Permission.Application,
                Id = (string)e.Permission.Id,
                Description = (string)e.Permission.Description
            }).ToList();
            return Task.FromResult(result);
        }

        private string GetSQLQueryForPermission(string application, string tenantId)
        {
            var query = $@"SELECT distinct c.Permission FROM c 
                where c.Permission.Application = '{application}' and c.TenantId='{tenantId}' ";
            return query;
        }

        private string GetSQLQueryForApplication(string tenantId)
        {
            var query = $"SELECT distinct c.Permission.Application FROM c where c.TenantId='{tenantId}'";
            return query;
        }
        private string GetSQLQueryForUser(IList<string> permissions, string userType, string application, string tenantId)
        {
            var query = $@"SELECT distinct c[""{userType}""].Key, c[""{userType}""].PersonalInfo FROM c WHERE c.Permission.Id IN ({ "\"" + string.Join("\", \"", permissions) + "\""}) and c.TenantId='{tenantId}' and c.Permission.Application='{application}' ";
            return query;
        }

        public Task<IList<Person>> GetUsers(IList<string> permissions, string userType, string application, string tenantId)
        {
            IList<Person> users = _client.CreateDocumentQuery<Person>(UriFactory.CreateDocumentCollectionUri(_dbConfig.Database, _dbConfig.Collection), GetSQLQueryForUser(permissions, userType,application,tenantId), _queryOptions).ToList();

            return Task.FromResult(users);
        }
        public Task<IList<string>> GetApplication(string tenantId)
        {
            var applications = _client.CreateDocumentQuery<dynamic>(UriFactory.CreateDocumentCollectionUri(_dbConfig.Database, _dbConfig.Collection), GetSQLQueryForApplication(tenantId), _queryOptions).ToList();
            IList<string> result = applications.Select(e => (string)e.Application).ToList();
            return Task.FromResult(result);
        }
    }
}
