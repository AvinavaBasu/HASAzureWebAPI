using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Raet.UM.HAS.Core.InitialLoad.Interface;
using System;

namespace Raet.UM.HAS.Server.InitialLoadFunction
{
    public class InitialLoadFunction
    {
        private readonly IInitialLoadBusiness _initialLoadBusiness;
        private readonly ILogger _logger;
        public InitialLoadFunction(IInitialLoadBusiness initialLoadBusiness, ILogger logger)
        {
            _initialLoadBusiness = initialLoadBusiness;
            _logger = logger;
        }

        [FunctionName("InitialLoad")]
        public void Run([QueueTrigger("initial-load", Connection = "AzureWebJobsStorage")]string fileUrl, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {fileUrl}");
            try
            {
                _initialLoadBusiness.Process(fileUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
          
        }
    }
}
