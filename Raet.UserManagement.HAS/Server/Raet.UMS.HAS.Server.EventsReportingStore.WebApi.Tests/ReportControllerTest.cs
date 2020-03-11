using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raet.UM.HAS.DTOs;
using System.IO;
using System.Threading.Tasks;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Mocks;
using Raet.UMS.HAS.Server.EventReportingStore.WebApi.Controllers;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Raet.UMS.HAS.Server.EventsReportingStore.WebApi.Tests
{

    public class CheckPropertyValidation
    {
        public IList<ValidationResult> myValidation(object model)
        {
            var result = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);
            Validator.TryValidateObject(model, validationContext, result);
            if (model is IValidatableObject) (model as IValidatableObject).Validate(validationContext);
            return result;
        }
    }

    [TestClass]
    public class ReportControllerTest
    {
        readonly MockInMemoryLogger logger = new MockInMemoryLogger();
        private ReportController EnrichedEventController;
        private Mock<IReportingBusiness> ReportingBusiness;

        [TestInitialize]
        public void SetupMocks()
        {
            ReportingBusiness = new Mock<IReportingBusiness>();
            EnrichedEventController = new ReportController(ReportingBusiness.Object, logger);
        }
        private ReportingEvent GetReportEventDto()
        {
            return new ReportingEvent
            {
                FileName = "AppReport.csv",
                Application = "App1",
                Permissions = new List<string> { "Read", "Write" },
                SourceUser = new ExternalId() { Context = "Youforce", Id = "IC01" },
                TargetUser = new ExternalId()
            };
        }

        private ReportingEvent ReportingEventDtoSetup()
        {
            var reportingEventDto = new ReportingEvent
            {
                Application = "DummyApplication",
                Permissions = new List<string> { "DummyPermission" },
                EndDate = DateTime.Now.AddDays(-1),
                StartDate = DateTime.Now.AddDays(1),
                FileName = "DummyFile.csv",
                TenantId = "DummyTenant",
                SourceUser = new ExternalId() { Context = "Youforce", Id = "IC01" },
                TargetUser= new ExternalId()
            };
            return reportingEventDto;
        }

        [TestMethod]

        public void GenerateReport_ReportingEventDto_ValidationIsSuccess_WhenAllRequiredFieldsProvided()
        {
            CheckPropertyValidation cpv = new CheckPropertyValidation();
            var request = ReportingEventDtoSetup();
            var errorCount = cpv.myValidation(request).Count();
            AreEqual((errorCount), 0);
        }

        [TestMethod]

        public void GenerateReport_ReportingEventDto_ValidationIsFailed_WhenAllApplicationNameNotProvided()
        {
            CheckPropertyValidation cpv = new CheckPropertyValidation();
            var request = new ReportingEvent();
            var errorCount = cpv.myValidation(request);
            AreEqual((errorCount.Count), 5);
            AreEqual(errorCount.ToList()[0].ErrorMessage, "The Application field is required.");
            AreEqual(errorCount.ToList()[1].ErrorMessage, "The Permissions field is required.");
            AreEqual(errorCount.ToList()[2].ErrorMessage, "The EndDate field is required.");
            AreEqual(errorCount.ToList()[3].ErrorMessage, "The FileName field is required.");
            AreEqual(errorCount.ToList()[4].ErrorMessage, "The TenantId field is required.");
        }

        [TestMethod]
        public void GetHeartBeat()
        {
            var result = EnrichedEventController.HeartBeat();
            AreEqual(((ObjectResult)result).Value, "Raet Historical Authorization Store Enriched Data WebApi");
            AreEqual(((ObjectResult)result).StatusCode, 200);
        }

        [TestMethod]
        public void GenerateReport_FailsIfBothSourceAndTargetAreSelected()
        {
            var request = ReportingEventDtoSetup();
            request.TargetUser = new ExternalId() { Context = "Raet", Id = "IC02" };
            request.EndDate = DateTime.Now.AddDays(-2);
            var result = EnrichedEventController.Generate(request).Result;
            AreEqual(((ObjectResult)result).StatusCode, 400);
            AreEqual(((ObjectResult)result).Value, "Both Source and Target user cannot be selected");
        }

        [TestMethod]
        public void GenerateReport_FailsIfStartDateIsGreaterThanEndDate()
        {
            var request = ReportingEventDtoSetup();
            request.EndDate= DateTime.Now.AddDays(-2);
            var result = EnrichedEventController.Generate(request).Result;
            AreEqual(((ObjectResult)result).StatusCode, 400);
            AreEqual(((ObjectResult)result).Value, "Start Date cannot be greater than End Date!");
        }

        [TestMethod]
        public void GenerateReport_GivenValidReprotDetailsShouldReturnSuccess()
        {
            var request = GetReportEventDto();
            var resultContent = new GenerateReport { FileName = request.FileName, Hash="Hash"  };
            ReportingBusiness.Setup(x => x.InsertAndTriggerGenerateReport(It.IsAny<ReportingEvent>())).Returns(Task.FromResult(resultContent));
            var generatedReport = EnrichedEventController.Generate(request).Result;
            AreEqual(((ObjectResult)generatedReport).StatusCode, 200);
        }

        [TestMethod]
        public void GenerateReport_ExceptionFromBusinessLogicShouldReturnInternalServerError()
        {
            var request = GetReportEventDto();
            ReportingBusiness.Setup(x => x.InsertAndTriggerGenerateReport(It.IsAny<ReportingEvent>()))
                .Throws(new IOException());

            var generatedReport = EnrichedEventController.Generate(request).Result;

            AreEqual(((StatusCodeResult)generatedReport).StatusCode, 500);
        }
        

        [TestMethod]
        public void Download_WhenDownloadDataReturnsNullNoCsvFileGenerated()
        {
            ReportingBusiness.Setup(x => x.GetDownloadStream(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new DownloadReport()));

            var generateReport = (StatusCodeResult)EnrichedEventController.Download(It.IsAny<string>(),It.IsAny<string>()).Result;

            AreEqual(generateReport.StatusCode, 500);
        }

        [TestMethod]
        public void Download_WhenValidInputShouldDownloadBlobToCsvFile()
        {
            var downloadedReport = new DownloadReport()
            {
                FileName = "DummyFile.csv",
                Stream = new MemoryStream()
            };
            ReportingBusiness.Setup(x => x.GetDownloadStream(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(downloadedReport));

            var generateReport = (FileStreamResult)EnrichedEventController.Download(It.IsAny<string>(), It.IsAny<string>()).Result;

            IsNotNull(generateReport);
            AreEqual(generateReport.FileDownloadName, downloadedReport.FileName);
            AreEqual(generateReport.ContentType, "application/csv");
        }

        [TestMethod]
        public void Download_WhenFileStreamReturnsNullDownloadBlobToCsvFileFails()
        {
            ReportingBusiness.Setup(x => x.GetDownloadStream(It.IsAny<string>(), It.IsAny<string>())).Throws<ArgumentException>();

            var generateReport = (StatusCodeResult)EnrichedEventController.Download(It.IsAny<string>(),It.IsAny<string>()).Result;

            AreEqual(generateReport.StatusCode, 500);
        }

        [TestMethod]
        public void Get_WhenValidInputGetFileRecords()
        {
            var fileInformation = new FileInformationDto() { FileName = "DummyFile.csv" };

            List<FileInformationDto> fileInformationDtos = new List<FileInformationDto>();
            fileInformationDtos.Add(fileInformation);

            var file = fileInformationDtos.AsEnumerable();

            ReportingBusiness.Setup(x => x.GetDownloadFileRecords(It.IsAny<string>())).Returns(Task.FromResult(file));

            var get = (OkObjectResult)EnrichedEventController.Get(It.IsAny<string>()).Result;

            AreEqual(get.StatusCode, 200);
        }

        [TestMethod]
        public void Get_WhenInValidInputGetFileRecordsThrowsError()
        {
            ReportingBusiness.Setup(x => x.GetDownloadFileRecords(It.IsAny<string>())).Throws<Exception>();

            var get = (StatusCodeResult)EnrichedEventController.Get(It.IsAny<string>()).Result;

            AreEqual(get.StatusCode, 500);
        }

        [TestMethod]
        public void GetPersonalInfo_WhenValidReturnsPersonalInfo()
        {
            var getPersonalInfo = EnrichedEventController.GetPersonalInfo();
            AreEqual(getPersonalInfo.LastNameAtBirth, "LastNameAtBirth");

        }
    }
}
