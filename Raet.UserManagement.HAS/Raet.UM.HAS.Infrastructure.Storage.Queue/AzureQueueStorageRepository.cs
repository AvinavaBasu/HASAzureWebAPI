using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using System;

namespace Raet.UM.HAS.Infrastructure.Storage.Queue
{
    public class AzureQueueStorageRepository : IAzureQueueStorageRepository
    {
        public readonly CloudQueue _cloudQueueContainer;
        private readonly ILogger _logger;


        public AzureQueueStorageRepository(IAzureQueueStorageInitializer initializer, ILogger logger)
        {
            _logger = logger;
            try
            {
                _cloudQueueContainer = initializer.Initialize();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error on queue initilization. Exception : {ex.Message}");
                throw;
            }
        }

        public async void AddAsync(string message)
        {
            CloudQueueMessage queueMessage = new CloudQueueMessage(message);
            try
            {
                await _cloudQueueContainer.AddMessageAsync(queueMessage);
                _logger.LogInformation($"Added Message to the report-file-queue: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception {ex.InnerException} occured while adding message to the report-file-queue: {message}");
                throw ex;
            }
        }
    }
}
