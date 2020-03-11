using Raet.UM.HAS.DTOs;
using System.Threading.Tasks;

namespace Reat.UM.HAS.Core.GenerateReport.Interface
{
    public interface IGenerateReportBusiness
    {
        Task FetchAndDownloadReport(string reportingEventDto);
    }
}