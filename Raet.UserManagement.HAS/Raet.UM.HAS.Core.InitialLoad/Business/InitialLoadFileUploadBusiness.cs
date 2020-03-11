using Hangfire;
using Raet.UM.HAS.Core.InitialLoad.Interface;
using Raet.UM.HAS.Infrastructure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Core.InitialLoad.Business
{
    public class InitialLoadFileUploadBusiness : IInitialLoadFileUploadBusiness
    {
        private IAzureBlobStorageRepository _azureBlobStorageRepository { get; set; }
        private IInitialLoadBusiness _initialLoadBusiness { get; set; }
        public InitialLoadFileUploadBusiness(IAzureBlobStorageRepository azureBlobStorageRepository, IInitialLoadBusiness initialLoadBusiness)
        {
            _azureBlobStorageRepository = azureBlobStorageRepository;
            _initialLoadBusiness = initialLoadBusiness;
        }

        public async Task UploadFileToBlob(Stream stream, string fileName)
        {
            var url = await _azureBlobStorageRepository.InitialLoadUploadFileAsync(stream, fileName);
            var jobId = BackgroundJob.Enqueue(() =>_initialLoadBusiness.Process(fileName));
            //_initialLoadBusiness.Process(fileName);
        }
    }
}
