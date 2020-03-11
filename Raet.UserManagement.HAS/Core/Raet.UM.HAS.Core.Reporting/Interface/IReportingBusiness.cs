using System.Collections.Generic;
using System.Threading.Tasks;
using Raet.UM.HAS.DTOs;
using Raet.UM.HAS.Infrastructure.Storage.Table.Model;
using DomainReportingEvent = Raet.UM.HAS.Core.Domain.ReportingEvent;

namespace Raet.UM.HAS.Core.Reporting.Interface
{
    public interface IReportingBusiness
    {
        Task<GenerateReport> GenerateReport(DomainReportingEvent reportingEvent);

        Task<DownloadReport> GetDownloadStream(string tenantId,string guid);
        Task<IEnumerable<FileInformationDto>> GetDownloadFileRecords(string tenantId);
        Task<GenerateReport> InsertAndTriggerGenerateReport(ReportingEvent reportingEvent);
    }
}
