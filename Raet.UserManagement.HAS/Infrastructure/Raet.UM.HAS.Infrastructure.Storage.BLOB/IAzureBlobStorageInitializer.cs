using Microsoft.WindowsAzure.Storage.Blob;

namespace Raet.UM.HAS.Infrastructure.Storage.Blob
{
    public interface IAzureBlobStorageInitializer
    {
       CloudBlobContainer Initialize();
          
    }
}
