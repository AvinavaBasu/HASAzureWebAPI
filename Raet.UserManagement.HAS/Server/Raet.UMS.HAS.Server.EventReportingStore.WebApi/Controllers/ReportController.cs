using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Domain = Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.DTOs;
using System;
using System.Threading.Tasks;
using VismaRaet.API.AspNetCore.Authorization;

namespace Raet.UMS.HAS.Server.EventReportingStore.WebApi.Controllers
{
    [ApiController]
    [Authorization()]
    [Route("api/")]
    public class ReportController : ControllerBase
    {

        private readonly IReportingBusiness _reportingBusiness;
        private readonly ILogger _logger;

        public ReportController(IReportingBusiness reportingBusiness, ILogger logger)
        {
            _reportingBusiness = reportingBusiness;
            _logger = logger;
        }

        [HttpGet]
        [Route("[controller]/HeartBeat")]
        public IActionResult HeartBeat()
        {
            return Ok("Raet Historical Authorization Store Enriched Data WebApi");
        }

        [HttpPost]
        [Route("[controller]/Generate")]
        public async Task<IActionResult> Generate([FromBody] ReportingEvent reportingEventDto)
        {
            try
            {
                if (!(string.IsNullOrWhiteSpace(reportingEventDto.SourceUser.Id)|| string.IsNullOrWhiteSpace(reportingEventDto.SourceUser.Context))
                   && !(string.IsNullOrWhiteSpace(reportingEventDto.TargetUser.Id) || string.IsNullOrWhiteSpace(reportingEventDto.TargetUser.Context)))
                {
                    return BadRequest("Both Source and Target user cannot be selected");
                }
                if (reportingEventDto.StartDate>reportingEventDto.EndDate)
                {
                    return BadRequest("Start Date cannot be greater than End Date!");
                }
                _logger.LogInformation("Generate Report process started!");
                var generatedReport = await _reportingBusiness.InsertAndTriggerGenerateReport(reportingEventDto);
                return Ok(generatedReport);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("[controller]/Download")]
        public async Task<IActionResult> Download(string tenantId,string guid)
        {
            try
            {
                var downloadReport = await _reportingBusiness.GetDownloadStream(tenantId,guid);
                var contentType = "application/csv";
                var file = File(downloadReport.Stream, contentType);
                file.FileDownloadName = downloadReport.FileName;
                return file;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet]
        [Route("[controller]/Get/{tenantId}")]
        public async Task<IActionResult> Get(string tenantId)
        {
            try
            {
                var result = await _reportingBusiness.GetDownloadFileRecords(tenantId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet]
        [Route("[controller]/GetPersonInfo")]
        public Domain.PersonalInfo GetPersonalInfo()
        {
            
                return new Domain.PersonalInfo
                {
                    BirthDate = DateTime.Now,
                    Initials = string.Concat("Pro", 00, 1),
                    LastNameAtBirth = "LastNameAtBirth",
                    LastNameAtBirthPrefix = new Random().Next(1, 10).ToString()
                };
               
        }
    }
}