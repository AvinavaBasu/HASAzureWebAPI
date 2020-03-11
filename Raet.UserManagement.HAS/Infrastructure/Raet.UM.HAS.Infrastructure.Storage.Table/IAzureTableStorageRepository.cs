using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Raet.UM.HAS.Infrastructure.Storage.Table.Model;

namespace Raet.UM.HAS.Infrastructure.Storage.Table
{
    public interface IAzureTableStorageRepository<T> where T : TableEntity
    {
        Task<TableResult> InsertRecordToTable(TableEntity obj);
        Task<T> RetrieveRecord(string partitionKey, string rowKey);
        Task<TableResult> DeleteRecord(TableEntity obj);
        Task<IEnumerable<T1>> RetrieveAllRecords<T1>(string partitionKey) where T1 : TableEntity, new();
        Task<TableResult> UpdateRecord(TableEntity obj);
    }
}