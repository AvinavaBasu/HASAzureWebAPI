using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;


namespace Raet.UM.HAS.Infrastructure.Storage.Table
{
    public class AzureTableStorageInitializer : IAzureTableStorageInitializer
    {
        private readonly ITableStorageSettings _settings;

        public AzureTableStorageInitializer(ITableStorageSettings settings) {

            if (string.IsNullOrEmpty(settings?.ConnectionString)) throw new ArgumentNullException(nameof(settings));
            _settings = settings;
        }
        public  CloudTable Initialize(string tableName) {
            
                var cloudStorageAccount = CloudStorageAccount.Parse(_settings.ConnectionString);
                var tableClient = cloudStorageAccount.CreateCloudTableClient();
                var cloudTable = tableClient.GetTableReference(tableName);
                cloudTable.CreateIfNotExistsAsync();
                return cloudTable;
        }
    }
}
