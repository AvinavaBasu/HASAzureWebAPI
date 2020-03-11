using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Infrastructure.Storage.CosmosDB
{
    public abstract class CosmosDBRepository
    {
        protected ICosmoDBSettings _dbConfig;

        protected DocumentClient _client;

        protected ILogger _logger;

        protected readonly FeedOptions _queryOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true, MaxBufferedItemCount=-1,MaxDegreeOfParallelism= -1, EnableScanInQuery=true};

        public async Task DeleteDocumentAsync(string documentId, RequestOptions requestOptions)
        {
           await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_dbConfig.Database, _dbConfig.Collection, documentId), requestOptions);
        }
    }
}
