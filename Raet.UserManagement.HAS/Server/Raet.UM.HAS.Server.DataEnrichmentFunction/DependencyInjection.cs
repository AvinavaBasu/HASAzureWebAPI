using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Infrastructure.Storage.CosmosDB;
using Raet.UM.HAS.Infrastructure.Storage.Table;
using Raet.UM.HAS.Infrastructure.Storage.Table.Storages;

namespace Raet.UM.HAS.Server.DataEnrichmentFunction
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConfigurations<T>(this IServiceCollection sc, IConfiguration configuration,
            string key) where T : class
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

            return sc;
        }

        private static ICosmoDBStorageInitializer GetCosmoCollectionSettings(
            ICosmoDBSettings settings)
        {
            return new CosmoDBStorageInitializer(settings);
        }
    }
}
