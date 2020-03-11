using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Raet.UM.HAS.Core.Reporting;
using Raet.UM.HAS.Core.Reporting.Business;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Infrastructure.Storage.Blob;
using Raet.UM.HAS.Infrastructure.Storage.CosmosDB;
using Raet.UM.HAS.Infrastructure.Storage.Queue;
using Raet.UM.HAS.Infrastructure.Storage.Table;
using Raet.UM.HAS.Infrastructure.Storage.Table.Model;
using Raet.UM.HAS.Infrastructure.Storage.Table.Storages;

namespace Raet.UM.HAS.Server.ReatHasGdprFunctionApps
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConfigurations<T>(this IServiceCollection sc, IConfiguration configuration, string key) where T : class
        {
            var t = configuration.GetSection(key).Get<T>();
            sc.Configure<T>(x => x = t);
            return sc;
        }

        public static IServiceCollection AddCustomLogging(this IServiceCollection sc)
        {
            sc.AddSingleton<ILogger>(c =>
            {
                var factory = c.GetRequiredService<ILoggerFactory>();
                var log = new Logger<GenerateReportFunction>(factory);
                return log;
            });
            sc.AddSingleton<ILogger>(c =>
            {
                var factory = c.GetRequiredService<ILoggerFactory>();
                var log = new Logger<DataEnrichmentFunction>(factory);
                return log;
            });
            return sc;
        }
        public static IServiceCollection AddCustomTableStorage<T>(this IServiceCollection sc) where T : TableEntity
        {
            sc.AddScoped(x => new AzureTableStorage<T>(x.GetRequiredService<ITableStorageSettings>()))

            .AddScoped<IAzureTableStorageRepository<T>>(c =>
            {
                var log = c.GetRequiredService<ILogger>();
                return c.GetRequiredService<AzureTableStorage<T>>().GetInstance(log);
            });
            return sc;
        }

        public static IServiceCollection AddStorageRepository(this IServiceCollection sc, IConfiguration configuration)
        {
            sc.AddScoped<IReadRawEventStorage>(c =>
            {
                var cosmosDbSettings = configuration.GetSection("CosmosReadDb").Get<CosmoDBSettings>();
                var log = c.GetRequiredService<ILogger>();
                return new RawEventStorageRepository(GetCosmoCollectionSettings(cosmosDbSettings), log);
            });

            sc.AddScoped<IReportingStorage>(c =>
            {
                var cosmosDbSettings = configuration.GetSection("CosmosWriteDb").Get<CosmoDBSettings>();
                var log = c.GetRequiredService<ILogger>();
                return new ReportingStorageRepository(GetCosmoCollectionSettings(cosmosDbSettings), log);
            });

            sc.AddScoped<IDataEnrichmentBusiness>(c =>
            {
                var cosmosReadDbSettings = configuration.GetSection("CosmosReadDb").Get<CosmoDBSettings>();
                var cosmosWriteDbSettings = configuration.GetSection("CosmosWriteDb").Get<CosmoDBSettings>();
                var log = c.GetRequiredService<ILogger>();
                var logFactory = c.GetRequiredService<ILoggerFactory>();
                var reportingStorage = c.GetRequiredService<IReportingStorage>();
                var effectiveAuthorizationHandlerFactory = c.GetRequiredService<IEffectiveAuthorizationHandlerFactory>();
                var userInformation = c.GetRequiredService<IUserInformation>();
                return new DataEnrichmentBusiness(GetEffectiveAuthorizationTimelineFactory(GetreadRawEventStorage(GetCosmoCollectionSettings(cosmosReadDbSettings), log),
                    effectiveAuthorizationHandlerFactory), GetReportingStorage(GetCosmoCollectionSettings(cosmosWriteDbSettings), log), userInformation, log);
            });
            return sc;
        }

        public static IServiceCollection AddCustomBlobStorageSettings(this IServiceCollection sc, IConfiguration configuration)
        {
            sc.AddScoped<IReportingBusiness>(c =>
            {
                var log = c.GetRequiredService<ILogger>();
                var reportingStorage = c.GetRequiredService<IReportingStorage>();
                var fileInformationRepository = c.GetRequiredService<IAzureTableStorageRepository<FileInformation>>();
                var checksumGenerator = c.GetRequiredService<IChecksumGenerator>();
                var azureQueueStorageRepository = c.GetRequiredService<IAzureQueueStorageRepository>();
                var generateReportBlobStorageSettings = configuration.GetSection("GenerateReportBlobStorageSettings").Get<BlobStorageSettings>();
                return new ReportingBusiness(reportingStorage, GetAzureBlobStorageRepository(GetAzureBlobStorageInitializer(generateReportBlobStorageSettings, log), log),
                    fileInformationRepository, checksumGenerator, generateReportBlobStorageSettings, azureQueueStorageRepository, log);
            });
            return sc;
        }

        private static IAzureBlobStorageInitializer GetAzureBlobStorageInitializer(IBLOBStorageSettings bLOBStorageSettings, ILogger logger)
        {
            return new AzureBlobStorageInitializer(bLOBStorageSettings, logger);
        }
        private static IAzureBlobStorageRepository GetAzureBlobStorageRepository(IAzureBlobStorageInitializer azureBlobStorageInitializer,
             ILogger logger)
        {
            return new AzureBlobStorageRepository(azureBlobStorageInitializer, logger);
        }
        private static IReportingStorage GetReportingStorage(ICosmoDBStorageInitializer clientInitializer, ILogger factory)
        {
            return new ReportingStorageRepository(clientInitializer, factory);
        }

        private static IReadRawEventStorage GetreadRawEventStorage(ICosmoDBStorageInitializer clientInitializer, ILogger factory)
        {
            return new RawEventStorageRepository(clientInitializer, factory);
        }

        private static IEffectiveAuthorizationTimelineFactory GetEffectiveAuthorizationTimelineFactory(IReadRawEventStorage eventStore,
            IEffectiveAuthorizationHandlerFactory eventHandlerFactory)
        {
            return new EffectiveAuthorizationTimelineFactory(eventStore, eventHandlerFactory);
        }

        private static ICosmoDBStorageInitializer GetCosmoCollectionSettings(
            ICosmoDBSettings settings)
        {
            return new CosmoDBStorageInitializer(settings);
        }
    }
}
