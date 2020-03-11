using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Raet.UM.HAS.Core.InitialLoad.Interface;
using Raet.UMS.HAS.Server.EventReportingStore.WebApi.Model;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using VismaRaet.API.AspNetCore.Authorization;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Raet.UMS.HAS.Server.EventReportingStore.WebApi.Controllers
{
    [ApiController]
    [Authorization()]
    [Route("api/")]
    public class InitialLoadController : ControllerBase
    {
        private readonly ILogger _logger;

        private readonly IInitialLoadFileUploadBusiness _initialLoadFileUploadBusiness;

        public InitialLoadController(IInitialLoadFileUploadBusiness initialLoadFileUploadBusiness, ILogger logger)
        {
            _logger = logger;
            _initialLoadFileUploadBusiness = initialLoadFileUploadBusiness;
        }


        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("[controller]/upload")]
        public async Task<IActionResult> Upload([FromForm]FileUpload file)
        {
            try
            {
                var currentFile = file.FormFile;

                if (currentFile.Length > 0)
                {
                    var stream = currentFile.OpenReadStream();
                    await _initialLoadFileUploadBusiness.UploadFileToBlob(stream, currentFile.FileName);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest("The File was not uploaded successfully");
            }
        }
    }
}