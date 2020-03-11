using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting.Interface;
using Reat.UM.HAS.Core.GenerateReport.Interface;
using System;
using System.Threading.Tasks;

namespace Reat.UM.HAS.Core.GenerateReport.Business
{
    public class GenerateReportBusiness : IGenerateReportBusiness
    {

        public IReportingBusiness _reportingBusiness { get; set; }
        private readonly ILogger _logger;


        public GenerateReportBusiness(IReportingBusiness reportingBusiness, ILogger logger)
        {
            _reportingBusiness = reportingBusiness;
            _logger = logger;
        }


        public async Task FetchAndDownloadReport(string report)
        {
            try
            {
                var reportingEvent = JsonConvert.DeserializeObject<ReportingEvent>(report);
                if (reportingEvent.EndDate == null)
                { reportingEvent.EndDate = DateTime.Now; }
                await _reportingBusiness.GenerateReport(reportingEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

        }
    }
}