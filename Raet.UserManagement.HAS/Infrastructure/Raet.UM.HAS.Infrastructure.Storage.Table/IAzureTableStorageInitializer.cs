using Microsoft.WindowsAzure.Storage.Table;

namespace Raet.UM.HAS.Infrastructure.Storage.Table
{
    public interface IAzureTableStorageInitializer
    {
        CloudTable Initialize(string tableName);
    }
}