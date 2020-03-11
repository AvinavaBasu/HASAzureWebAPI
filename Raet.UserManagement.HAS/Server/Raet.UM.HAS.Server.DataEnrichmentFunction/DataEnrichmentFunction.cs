// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Raet.UM.HAS.Core.Reporting.Interface;


namespace Raet.UM.HAS.Server.DataEnrichmentFunction
{
    public class DataEnrichmentFunction
    {
        private readonly IDataEnrichmentBusiness _enrichmentBusiness;
        private readonly ILogger _logger;

        public DataEnrichmentFunction(IDataEnrichmentBusiness enrichmentBusiness, ILogger logger)
        {
            _enrichmentBusiness = enrichmentBusiness;
            _logger = logger;
        }

        [FunctionName("DataEnrichmentFunction")]
        public void Run([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());
            _enrichmentBusiness.Process(eventGridEvent.Data.ToString());
            log.LogInformation("Data Enrichment process completed");
        }
    }
}
