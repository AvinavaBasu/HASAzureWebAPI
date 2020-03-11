using System;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;


namespace Raet.UM.HAS.Infrastructure.Storage.Queue
{
    public class AzureQueueStorageInitializer : IAzureQueueStorageInitializer
    {
        private readonly IQueueStorageSettings _settings;

        private readonly ILogger _logger;
        private CloudQueue _cloudQueueContainer { get; set; }


        public AzureQueueStorageInitializer(IQueueStorageSettings settings, ILogger logger)
        {

            if (string.IsNullOrEmpty(settings?.ConnectionString)) throw new ArgumentNullException(nameof(settings));
            _settings = settings;
            _logger = logger;
        }


        public CloudQueue Initialize()
        {
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_settings.ConnectionString);
                CloudQueueClient cloudQueueClient = cloudStorageAccount.CreateCloudQueueClient();
                _cloudQueueContainer = cloudQueueClient.GetQueueReference(_settings.ContainerName);
                _cloudQueueContainer.CreateIfNotExistsAsync().Wait();
                return _cloudQueueContainer;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Queuecontainer not initialized due to:{ex.Message}");
                throw new ArgumentNullException("Storage unavailable");
            }
        }
    }
}
