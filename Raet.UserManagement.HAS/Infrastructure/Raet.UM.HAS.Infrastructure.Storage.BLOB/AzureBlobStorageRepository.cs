using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Infrastructure.Storage.Blob
{
    public class AzureBlobStorageRepository : IAzureBlobStorageRepository
    {
        public readonly CloudBlobContainer _cloudBlobContainer;
        private readonly ILogger _logger;


        public AzureBlobStorageRepository(IAzureBlobStorageInitializer initializer, ILogger logger)
        {
            _logger = logger;
            try
            {
                _cloudBlobContainer = initializer.Initialize();
            }
            catch (Exception ex)
            {
                logger.LogError($"container not initialized due to:{ex.Message}");
                throw new ArgumentNullException("Storage unavailable");
            }
        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            CloudBlockBlob _blockBlob = _cloudBlobContainer.GetBlockBlobReference(fileName);
            var result = await _blockBlob.DeleteIfExistsAsync();
            return result;
        }

        public async Task<Stream> FetchFileAsync(string fileName)
        {
            var cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(fileName);
            cloudBlockBlob.Properties.ContentType = "application/octet-stream";
            var stream = await cloudBlockBlob.OpenReadAsync();
            return stream;
        }

        public async Task<Uri> UploadFileAsync(string fileToBeUploaded, string fileName)
        {
            CloudBlockBlob cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(fileName);
            cloudBlockBlob.Properties.ContentType = "text/csv; charset=utf-8";
            await cloudBlockBlob.UploadTextAsync(fileToBeUploaded);
            return cloudBlockBlob.Uri;
        }

        public async Task<Uri> InitialLoadUploadFileAsync(Stream fileToBeUploaded, string fileName)
        {
            CloudBlockBlob cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(fileName);
            cloudBlockBlob.Properties.ContentType = "application/octet-stream";
            await cloudBlockBlob.UploadFromStreamAsync(fileToBeUploaded);
            return cloudBlockBlob.Uri;
        }
    }
}
