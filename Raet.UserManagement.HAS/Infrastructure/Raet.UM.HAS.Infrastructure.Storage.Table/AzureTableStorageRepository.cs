using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Raet.UM.HAS.Infrastructure.Storage.Table
{
    public class AzureTableStorageRepository<T> : IAzureTableStorageRepository<T> where T: TableEntity
    {
        private readonly CloudTable cloudTable;
        private readonly ILogger _logger;


        public AzureTableStorageRepository( IAzureTableStorageInitializer initializer, ILogger logger )
        {
            _logger = logger;

            try
            {
                cloudTable = initializer.Initialize(typeof(T).Name);
            }
            catch (Exception ex) {
                logger.LogError($"Storage not initialized due to:{ex.Message}");
                throw new ArgumentNullException("Storage unavailable");
            }
        }

        public async Task<IEnumerable<T1>> RetrieveAllRecords<T1>(string partitionKey) where T1 : TableEntity, new()
        {
            TableQuery<T1> fileRecordQuery = new TableQuery<T1>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));            
            var result = await cloudTable.ExecuteQuerySegmentedAsync(fileRecordQuery, null);
            var resultSet = result.OrderByDescending(r => r.Timestamp).Take(10);
            return resultSet;   
        }
        public async Task<T> RetrieveRecord(string partitionKey, string rowKey)
        {
            TableResult tableResult=null;
            try
            {
                var tableOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
                tableResult = await cloudTable.ExecuteAsync(tableOperation);
            }
            catch (StorageException ex)
            {
                var requestInformation = ex.RequestInformation;
                toLog("Retrieve", typeof(T).Name, partitionKey, rowKey, requestInformation.HttpStatusCode);
                throw ex;
            }

            return tableResult?.HttpStatusCode == 200? tableResult.Result as T: null;
        }

        public async Task<TableResult> InsertRecordToTable(TableEntity obj)
        {
            TableResult tableResult=null;
            try
            {
                var tableOperation = TableOperation.Insert(obj);
                tableResult = await cloudTable.ExecuteAsync(tableOperation);
            }
            catch (StorageException ex){
                 var requestInformation = ex.RequestInformation;
                 toLog("Insert ", typeof(T).Name, obj.PartitionKey, obj.RowKey, requestInformation.HttpStatusCode);
            }
           
            return tableResult;
        }

        public async Task<TableResult> UpdateRecord(TableEntity obj)
        {
            TableResult tableResult = null;
            try
            {
                var tableOperation = TableOperation.Retrieve(obj.PartitionKey, obj.RowKey);
                tableResult = await cloudTable.ExecuteAsync(tableOperation);


                if (tableResult != null)
                {
                    var tableOperationUpdate = TableOperation.InsertOrReplace(obj);
                    tableResult = await cloudTable.ExecuteAsync(tableOperationUpdate);
                }
            }
            catch (StorageException ex)
            {
                var requestInformation = ex.RequestInformation;
                toLog("Update", typeof(T).Name, obj.PartitionKey, obj.RowKey, requestInformation.HttpStatusCode);
                throw ex;
            }

            return tableResult;
        }

        public async Task<TableResult> DeleteRecord(TableEntity obj) {
            TableResult tableResult=null;
            try
            {
                obj.ETag = "*";
                TableOperation tableOperation = TableOperation.Delete(obj);
                tableResult = await cloudTable.ExecuteAsync(tableOperation);
            }
            catch (StorageException ex){
                var requestInformation = ex.RequestInformation;
                toLog("Delete", typeof(T).Name, obj.PartitionKey, obj.RowKey, requestInformation.HttpStatusCode);
            }

            return tableResult;
        }

        private void  toLog(string operationType, string entityType, string partKey, string rowKey, int StatusCode) {
            _logger.LogError($" { operationType } Storage operation for { entityType } with id ({ partKey },{ rowKey }) returned an { StatusCode.ToString() } response");
        }
        

    }
}
