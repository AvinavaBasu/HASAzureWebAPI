using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace Raet.UM.HAS.Infrastructure.Storage.Table.Storages
{
    public class AzureTableStorage<T> where T : TableEntity
    {
        private ITableStorageSettings _configuration;

        public AzureTableStorage(ITableStorageSettings config)
        {
            _configuration = config;
        }

        public  AzureTableStorageRepository<T> GetInstance(ILogger log)
        {
            var initializer = new AzureTableStorageInitializer(_configuration);
            var azureStorageRepository = new AzureTableStorageRepository<T>(initializer, log);
            return azureStorageRepository;
        }
    }
}
