using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Reat.UM.HAS.Core.GenerateReport.Interface;

namespace Reat.UM.HAS.Server.GenerateReportFunction
{
    public class GenerateReportFunction
    {
        private readonly IGenerateReportBusiness _generateReport;
        private readonly ILogger _logger;

        public GenerateReportFunction(IGenerateReportBusiness generateReport, ILogger logger)
        {
            _generateReport = generateReport;
            _logger = logger;
        }

        [FunctionName("GenerateReport")]
        public void Run([QueueTrigger("%QueueName%", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            _generateReport.FetchAndDownloadReport(myQueueItem);
        }
    }
}
