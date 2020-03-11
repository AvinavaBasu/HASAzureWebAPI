using System;
using Microsoft.AspNetCore.Mvc;
using VismaRaet.API.AspNetCore.Authorization;

namespace Raet.UM.HAS.Server.WebApi.Controllers
{
    public class DefaultController : Controller
    {
    
        [HttpGet]
        [Authorization("View HAS permission")]
        [Route("api/heartbeat")]
        public IActionResult Get()
        {
            return Ok("Raet Historical Authorization Store WebApi");
        }

        [HttpGet]
        [Authorization("View HAS permission")]
        [Route("api/PipelineTesting")]
        public IActionResult PipelineTest()
        {
            return Ok("This is a test for pipeline deployment");
        }
    }
}
