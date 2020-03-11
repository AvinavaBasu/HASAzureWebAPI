using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace Raet.UM.HAS.Infrastructure.Storage.CosmosDB
{
    public class CosmoDBStorageInitializer: ICosmoDBStorageInitializer 
    {
        public ICosmoDBSettings DbConfig { get; }
        private DocumentClient Client { get; set; }

        public CosmoDBStorageInitializer(ICosmoDBSettings dbconfig)
        {
            DbConfig = dbconfig;
        }

       public DocumentClient Initialize()
       {
            if (Client != null) return Client;
            Client = new DocumentClient(new Uri(DbConfig.Endpoint), DbConfig.AuthKey, new ConnectionPolicy()
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp,
                MaxConnectionLimit = 1000,
                RetryOptions = new RetryOptions() { MaxRetryAttemptsOnThrottledRequests = 50, MaxRetryWaitTimeInSeconds = 30}
            });
            
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
            Client.OpenAsync().Wait();
            return Client;
        }
      
        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await Client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DbConfig.Database));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await Client.CreateDatabaseAsync(new Database { Id = DbConfig.Database });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await Client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DbConfig.Database, DbConfig.Collection));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await Client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DbConfig.Database),
                        new DocumentCollection { Id = DbConfig.Collection },
                        new RequestOptions { OfferThroughput = 10000 });
                }
                else
                {
                    throw;
                }
            }
        }
             
        
    }
}
