using System.IO;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Core.InitialLoad.Interface
{
    public interface IInitialLoadFileUploadBusiness
    {
        Task UploadFileToBlob(Stream stream, string fileName);
    }
}
