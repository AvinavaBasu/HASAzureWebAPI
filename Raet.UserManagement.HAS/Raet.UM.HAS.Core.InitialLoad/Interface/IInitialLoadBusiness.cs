using System.Threading.Tasks;

namespace Raet.UM.HAS.Core.InitialLoad.Interface
{
    public interface IInitialLoadBusiness
    {
        Task Process(string fileUrl);
    }
}
