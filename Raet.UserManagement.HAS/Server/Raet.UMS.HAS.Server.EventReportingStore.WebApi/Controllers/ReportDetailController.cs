using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Raet.UM.HAS.Core.Reporting.Interface;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using VismaRaet.API.AspNetCore.Authorization;

namespace Raet.UMS.HAS.Server.EventReportingStore.WebApi.Controllers
{
    [ApiController]
    [Authorization()]
    [Route("api/")]
    public class ReportDetailController : ControllerBase
    {

        private readonly IEAAggregateBusiness _EAAggregateBusiness;
        private readonly ILogger _Logger;

        public ReportDetailController(IEAAggregateBusiness EAAggregateBusiness, ILogger logger)
        {
            _EAAggregateBusiness = EAAggregateBusiness;
            _Logger = logger;
        }

        [HttpGet]
        [Route("[controller]/Application/{tenantId}")]
        public async Task<IActionResult> Application(string tenantId)
        {
            try
            {
                var applications = await _EAAggregateBusiness.GetApplication(tenantId);
                if (!applications.Any())
                {
                    return NoContent();
                }
                return Ok(applications);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("[controller]/Permission/{application}/{tenantId}")]
        public async Task<IActionResult> Permission( string application, string tenantId)
        {
            try
            {
               var permissions = await _EAAggregateBusiness.GetPermissionData(application,tenantId);
                if (!permissions.Any())
                    return NoContent();
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        [Route("[controller]/SourceUser/{application}/{tenantId}")]
        public async Task<IActionResult> SourceUser([FromBody] IList<string> permissions, string application, string tenantId)
        {
            try
            {
                var sourceUsers = await _EAAggregateBusiness.GetUsers(permissions, "User",application, tenantId);
                if (!sourceUsers.Any())
                {
                    return NoContent();
                }
                return Ok(sourceUsers);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        [Route("[controller]/TargetUser/{application}/{tenantId}")]
        public async Task<IActionResult> TargetUser([FromBody] IList<string> permissions, string application, string tenantId)
        {
            try
            {
                var targetUsers = await _EAAggregateBusiness.GetUsers(permissions, "TargetPerson",application,tenantId);
                if (!targetUsers.Any())
                {
                    return NoContent();
                }
                return Ok(targetUsers);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
