using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProtoBuf;
using ProtoBuf.Meta;
using Raet.UM.HAS.Core.Application;
using Raet.UM.HAS.Core.InitialLoad.Interface;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Infrastructure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Core.InitialLoad.Business
{
    public class InitialLoadBusiness : IInitialLoadBusiness
    {
        private IAzureBlobStorageRepository _azureBlobStorageRepository { get; set; }
        private IWriteRawEventStorage _writeRawEventStorage { get; set; }
        private IDataEnrichmentBusiness _dataEnrichmentBusiness { get; set; }
        private readonly ILogger _logger;
        private RuntimeTypeModel _model;
        public InitialLoadBusiness(IAzureBlobStorageRepository azureBlobStorageRepository, IWriteRawEventStorage writeRawEventStorage, IDataEnrichmentBusiness dataEnrichmentBusiness, ILogger logger)
        {
            _azureBlobStorageRepository = azureBlobStorageRepository;
            _writeRawEventStorage = writeRawEventStorage;
            _dataEnrichmentBusiness = dataEnrichmentBusiness;
            _model = ModelBuilder.GetModel();
            _model.CompileInPlace();
            _logger = logger;
        }
        public async Task Process(string fileUrl)
        {
            var allTasks = new List<Task>();
            var semaphore = new SemaphoreSlim(10, 10);
            var filecount = 0;
            var data = await _azureBlobStorageRepository.FetchFileAsync(fileUrl);
            var events = Serializer.Deserialize<List<DTOs.EffectiveAuthorizationGrantedEvent>>(data);
            _logger.LogInformation($"Process Start Time {DateTime.Now}");
            events.ForEach(async (eventData) =>
            {
                var mapEventData = DomainAdapter.MapEvent(eventData);
                var serlializedJson = "";
                await semaphore.WaitAsync();
                allTasks.Add(
                    Task.Run(async () =>
                        {
                            try
                            {
                                Interlocked.Increment(ref filecount);
                                await _writeRawEventStorage.WriteRawEventAsync(mapEventData);
                                serlializedJson = JsonConvert.SerializeObject(mapEventData);
                                _dataEnrichmentBusiness.Process(serlializedJson.ToString());
                                _logger.LogInformation($"Data Enrichment Count : {filecount}");
                            }
                            catch(Exception ex)
                            {
                                _logger.LogError($"The Initial Load Failed due to : {ex.ToString()}");
                                _logger.LogError($"The failed Event Data is : {eventData.EffectiveAuthorization},{eventData.FromDateTime},{eventData.Id}");
                            }
                            finally
                            {
                                semaphore.Release();
                                _logger.LogInformation($"Released in finally");

                            }
                        }));
            });
            await Task.WhenAll(allTasks);
            _logger.LogInformation($"Data Enrichment Completed");
            _logger.LogInformation($"Process End Time {DateTime.Now}");


        }
    }
}
