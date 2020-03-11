using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Infrastructure.Storage.Blob
{
    public interface IAzureBlobStorageRepository
    {
        Task<Uri> UploadFileAsync(string fileToBeUploaded, string fileName);
        Task<Stream> FetchFileAsync(string fileName);
        Task<bool> DeleteFileAsync(string fileName);
        Task<Uri> InitialLoadUploadFileAsync(Stream fileToBeUploaded, string fileName);
    }
}
