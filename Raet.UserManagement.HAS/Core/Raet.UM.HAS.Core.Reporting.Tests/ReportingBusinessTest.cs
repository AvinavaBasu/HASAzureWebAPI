using AutoMapper;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting.Business;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Infrastructure.Storage.Blob;
using Raet.UM.HAS.Infrastructure.Storage.CosmosDB;
using Raet.UM.HAS.Infrastructure.Storage.Table;
using Raet.UM.HAS.Infrastructure.Storage.Table.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Raet.UM.HAS.Core.Reporting.Tests.Helper;
using Xunit;
using Raet.UM.HAS.Infrastructure.Storage.Queue;
using Microsoft.Extensions.Logging;

namespace Raet.UM.HAS.Core.Reporting.Tests
{
    public class ReportingBusinessTest
    {
        private Mock<IReportingStorage> _reportingStorageMock;
        private Mock<IAzureBlobStorageRepository> _azureBlobStorageRepMock;
        private Mock<IAzureTableStorageRepository<FileInformation>> _azureTableStorageRepMock;
        private Mock<IChecksumGenerator> _checkSumGeneratorMock;
        private Mock<IBLOBStorageSettings> _blobStorageSettingsMock;
        private Mock<IRuntimeMapper> _mapperMock;
        private Mock<IAzureQueueStorageRepository> _azureQueueStoragerRepMock;
        private Mock<ILogger> _logger;

        private ReportingBusiness _reportingBusiness;
        private ReportingEvent _reportingEvent;

        public ReportingBusinessTest()
        {
            _reportingStorageMock = new Mock<IReportingStorage>();
            _azureBlobStorageRepMock = new Mock<IAzureBlobStorageRepository>();
            _azureTableStorageRepMock = new Mock<IAzureTableStorageRepository<FileInformation>>();
            _checkSumGeneratorMock = new Mock<IChecksumGenerator>();
            _blobStorageSettingsMock = new Mock<IBLOBStorageSettings>();
            _mapperMock = new Mock<IRuntimeMapper>();
            _azureQueueStoragerRepMock = new Mock<IAzureQueueStorageRepository>();
            _logger = new Mock<ILogger>();

        }

        [Fact]
        public async Task GenerateReport_TestAsync()
        {
            //Arrange

            _blobStorageSettingsMock.SetupProperty(e => e.MaxDataLimitForBlob,"100");

            _reportingBusiness = new ReportingBusiness(_reportingStorageMock.Object, _azureBlobStorageRepMock.Object,
                _azureTableStorageRepMock.Object, _checkSumGeneratorMock.Object, _blobStorageSettingsMock.Object, _azureQueueStoragerRepMock.Object, _logger.Object);

            _reportingEvent = new ReportingEvent()
            {
                FileName = "TestFile"
            };


            //Register Mapper
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<EffectiveIntervalCSVMapperProfile>();
            });

            _mapperMock.Setup(e =>
                e.Map<IList<EffectiveAuthorizationInterval>, List<EffectiveIntervalCSVMapper>>(
                    Generator.GenerateEffectiveAuthorizationIntervals()))
                .Returns(It.IsAny<List<EffectiveIntervalCSVMapper>>());

            _reportingStorageMock.Setup(e => e.FetchReportingData(It.IsAny<ReportingEvent>()))
                .ReturnsAsync(Generator.GenerateEffectiveAuthorizationIntervals);

            _checkSumGeneratorMock.Setup(x => x.Generate(It.IsAny<string>())).Returns("Generated CheckSum");

            _azureBlobStorageRepMock.Setup(e => e.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new Uri("http://localhost")));

            _azureTableStorageRepMock.Setup(e => e.InsertRecordToTable(It.IsAny<FileInformation>()))
                .Returns(Task.FromResult(new TableResult()));


            //Act
            var result = await _reportingBusiness.GenerateReport(_reportingEvent);

            //Assert
            Assert.Equal(_reportingEvent.FileName, result.FileName);
            Assert.Equal("Generated CheckSum", result.Hash);


        }

        [Fact]
        public async Task GenerateReport_TestAsync_Exception()
        {
            //Arrange
            _reportingBusiness = new ReportingBusiness(_reportingStorageMock.Object, _azureBlobStorageRepMock.Object,
                _azureTableStorageRepMock.Object, _checkSumGeneratorMock.Object, _blobStorageSettingsMock.Object, _azureQueueStoragerRepMock.Object, _logger.Object);

            _reportingEvent = new ReportingEvent()
            {
                FileName = "TestFile"
            }; 

            _reportingStorageMock.Setup(e => e.FetchReportingData(It.IsAny<ReportingEvent>()))
                .ReturnsAsync(Generator.GenerateEffectiveAuthorizationIntervals);

            //Act
            var result = await _reportingBusiness.GenerateReport(_reportingEvent);

            //Assert
            Assert.Equal("Exceeds max record count, limit is 0 and actual count is 1", result.Hash);

        }


        [Fact]
        public async Task GetDownloadStreamTest()
        {
            //Arrange

            _reportingBusiness = new ReportingBusiness(_reportingStorageMock.Object, _azureBlobStorageRepMock.Object,
                _azureTableStorageRepMock.Object, _checkSumGeneratorMock.Object, _blobStorageSettingsMock.Object, _azureQueueStoragerRepMock.Object, _logger.Object);

            _azureTableStorageRepMock.Setup(e => e.RetrieveRecord(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new FileInformation() { FileName = "TestFile" }));

            _azureBlobStorageRepMock.Setup(e => e.FetchFileAsync(It.IsAny<string>()))
                .ReturnsAsync(new MemoryStream());

            //Act
            var result = await _reportingBusiness.GetDownloadStream("","");

            //Assert
            Assert.Equal("TestFile.csv", result.FileName);
            Assert.NotNull(result.Stream);

        }
    }
}
