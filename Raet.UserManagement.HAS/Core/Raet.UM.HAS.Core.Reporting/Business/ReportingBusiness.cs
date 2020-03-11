using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.DTOs;
using Raet.UM.HAS.Infrastructure.Storage.Blob;
using Raet.UM.HAS.Infrastructure.Storage.Queue;
using Raet.UM.HAS.Infrastructure.Storage.Table;
using Raet.UM.HAS.Infrastructure.Storage.Table.Model;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Core.Reporting.Business
{
    public class ReportingBusiness : IReportingBusiness
    {
        private IReportingStorage _reportingStorage { get; set; }
        private IAzureBlobStorageRepository _azureBlobStorageRepository { get; set; }
        private IAzureQueueStorageRepository _azureQueueStorageRepository { get; set; }
        private IChecksumGenerator _checksumGenerator { get; set; }
        private IAzureTableStorageRepository<FileInformation> _fileInformationRepository { get; set; }
        private readonly ILogger _logger;
        private readonly IBLOBStorageSettings _blobStorageSettings;
        private static int _maxCount;
        public ReportingBusiness(IReportingStorage reportingStorage, IAzureBlobStorageRepository azureBlobStorageRepository,
            IAzureTableStorageRepository<FileInformation> fileInformationRepository, IChecksumGenerator checksumGenerator,
            IBLOBStorageSettings blobStorageSettings, IAzureQueueStorageRepository azureQueueStorageRepository, ILogger logger)
        {
            _reportingStorage = reportingStorage;
            _azureBlobStorageRepository = azureBlobStorageRepository;
            _fileInformationRepository = fileInformationRepository;
            _azureQueueStorageRepository = azureQueueStorageRepository;
            _checksumGenerator = checksumGenerator;
            _blobStorageSettings = blobStorageSettings;
            _logger = logger;
            _maxCount = Convert.ToInt32(_blobStorageSettings.MaxDataLimitForBlob);
            SetCustomHeadersMap();
        }

        public async Task<GenerateReport> InsertAndTriggerGenerateReport(ReportingEvent reportingEventDto)
        {
            var reportingEvent = DomainAdapter.MapReportingEvent(reportingEventDto);
            _logger.LogInformation("Inserting record to the FileInformation Table");
            await _fileInformationRepository.UpdateRecord(FileInformation.Get(reportingEvent,new Uri("https://raetgdprtbldev.blob.core.windows.net"), "HashProcessing", "Processing"));
            string message = JsonConvert.SerializeObject(reportingEvent);
            _logger.LogInformation("Adding message to report-file-queue");
            _azureQueueStorageRepository.AddAsync(message);
            return new GenerateReport() { FileName = reportingEvent.FileName, Guid = reportingEvent.Guid };
        }

        public async Task<GenerateReport> GenerateReport(Domain.ReportingEvent reportingEvent)
        {
            try
            {
                var data = await _reportingStorage.FetchReportingData(reportingEvent);

                if (data.Count >= _maxCount)
                {
                    throw new ConstraintException(
                        $"Exceeds max record count, limit is {_maxCount} and actual count is {data.Count}");
                }

                var mapCSVData = Mapper.Map<IList<Domain.EffectiveAuthorizationInterval>, List<Domain.EffectiveIntervalCSVMapper>>(data);

                var csvData = CsvSerializer.SerializeToCsv(mapCSVData);

                var hash = _checksumGenerator.Generate(csvData);

                var url = await _azureBlobStorageRepository.UploadFileAsync(csvData, reportingEvent.FileNameTimeStamp);

                await _fileInformationRepository.UpdateRecord(FileInformation.Get(reportingEvent, url, hash, "Completed"));

                return new GenerateReport() { FileName = reportingEvent.FileName, Hash = hash };
            }

            catch(Exception ex)
            {
                _logger.LogError(ex.InnerException, ex.Message);
                await _fileInformationRepository.UpdateRecord(FileInformation.Get(reportingEvent, new Uri("https://raetgdprtbldev.blob.core.windows.net"), ex.Message, "Failed"));
                return new GenerateReport() { FileName = reportingEvent.FileName, Hash = ex.Message };
            }
        }

        private static void SetCustomHeadersMap()
        {
            JsConfig<DateTime>.SerializeFn = date => date.ToString("yyyy-MM-dd hh:mm:ss");

            CsvConfig<Domain.EffectiveIntervalCSVMapper>.CustomHeadersMap = new Dictionary<string, string>
            {
                {"EffectiveIntervalStart", "Effective Timespan Start" },
                {"EffectiveIntervalEnd", "Effective Timespan End" },
                {"UserKeyId", "Access User ID" },
                {"UserPersonalInfoInitials", "Access user initials" },
                {"UserPersonalInfoLastNameAtBirth", "Access user last name at birth" },
                {"UserPersonalInfoLastNameAtBirthPrefix", "Access user last name at birth prefix" },
                {"UserPersonalInfoBirthDate", "Access User Birth Date" },
                {"TargetKeyContext", "Target Type" },
                {"TargetKeyId", "Target User ID" },
                {"TargetPersonalInfoInitials", "Target User Initials" },
                {"TargetPersonalInfoLastNameAtBirth", "Target User Last Name At Birth" },
                {"TargetPersonalInfoLastNameAtBirthPrefix", "Target User Last Name At Birth Prefix" },
                {"TargetPersonalInfoBirthDate", "Target User Birth Date" },
                {"PermissionApplication", "Application" },
                {"PermissionId", "Permission" },
                {"PermissionDescription", "Permission Description" },
                { "TenantId", "Tenant" },
            };
        }

        public async Task<DownloadReport> GetDownloadStream(string tenantId,string guid)
        {
            var fileInfo = await _fileInformationRepository.RetrieveRecord(tenantId, guid);
            var stream = await _azureBlobStorageRepository.FetchFileAsync(fileInfo.FileNameTimeStamp);
            return new DownloadReport() { FileName = $"{fileInfo.FileName}.csv", Stream = stream };
        }
        public async Task<IEnumerable<DTOs.FileInformationDto>> GetDownloadFileRecords(string tenantId)
        {
            var fileInfo = await _fileInformationRepository.RetrieveAllRecords<FileInformation>(tenantId);
            return fileInfo.OrderByDescending(e => e.Timestamp).Select(e => new FileInformationDto
            {
                FileName = e.FileName,
                Guid = e.RowKey,
                Url = e.Url,
                Hash = e.Hash,
                Status = e.Status,
                CreatedAt = e.Timestamp.DateTime
            });
        }
    }
}

