using Microsoft.AspNetCore.Http;
namespace Raet.UMS.HAS.Server.EventReportingStore.WebApi.Model
{
    public class FileUpload
    {
        public IFormFile FormFile { get; set; }
    }
}
