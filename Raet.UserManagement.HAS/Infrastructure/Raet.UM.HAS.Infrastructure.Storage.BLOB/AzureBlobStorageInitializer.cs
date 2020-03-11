using System;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;


namespace Raet.UM.HAS.Infrastructure.Storage.Blob
{
    public class AzureBlobStorageInitializer : IAzureBlobStorageInitializer
    {
        private readonly IBLOBStorageSettings _settings;

        private readonly ILogger _logger;
        private CloudBlobContainer _cloudBlobContainer { get; set; }


        public AzureBlobStorageInitializer(IBLOBStorageSettings settings, ILogger logger)
        {

            if (string.IsNullOrEmpty(settings?.ConnectionString)) throw new ArgumentNullException(nameof(settings));
            _settings = settings;
            _logger = logger;
        }


        public CloudBlobContainer Initialize()
        {
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_settings.ConnectionString);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                _cloudBlobContainer = cloudBlobClient.GetContainerReference(_settings.ContainerName);
                _cloudBlobContainer.CreateIfNotExistsAsync().Wait();
                return _cloudBlobContainer;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Blobcontainer not initialized due to:{ex.Message}");
                throw new ArgumentNullException("Storage unavailable");
            }
        }
    }
}
