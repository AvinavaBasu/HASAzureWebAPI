using Microsoft.Extensions.Logging;
using Raet.UM.HAS.Core.Reporting;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Infrastructure.Http;
using Raet.UM.HAS.Infrastructure.Http.Common;
using Raet.UM.HAS.Infrastructure.Storage.Table;
using Raet.UM.HAS.Infrastructure.Storage.Table.Model;

namespace Raet.UM.HAS.Infrastructure.Integration.Tests
{
    public static class PersonalInfoEnrichmentServiceHelper
    {
        public static IPersonalInfoEnrichmentService BuildService(ILogger logger)
        {
            var settings = new TableStorageSettings()
            {
                ConnectionString = InfrastructureConfiguration.TableStorageConnString
            };

            var initializer = new AzureTableStorageInitializer(settings);

            var storedPersonalInfoRepo = new AzureTableStorageRepository<StoredPersonalInfo>(initializer, logger);
            var personLocalStorage = new PersonLocalStorage(storedPersonalInfoRepo);

            var contextMappingRepo = new AzureTableStorageRepository<ContextMapping>(initializer, logger);

            var contextMappingLocalStorage = new ContextMappingLocalStorage(contextMappingRepo);

            var authenticationSettings = new AuthenticationSettings(InfrastructureConfiguration.AuthProviderUri,
                                                                    InfrastructureConfiguration.AuthProviderClient,
                                                                    InfrastructureConfiguration.AuthProviderSecret);

            var authenticationProvider = new AuthenticationProvider(authenticationSettings);

            var personalInfoExternalServiceFactory = new PersonalInfoExternalServiceFactory(contextMappingLocalStorage, authenticationProvider, logger);

            
            return new PersonalInfoEnrichmentService(personLocalStorage, personalInfoExternalServiceFactory, logger);
        }
    }
}
