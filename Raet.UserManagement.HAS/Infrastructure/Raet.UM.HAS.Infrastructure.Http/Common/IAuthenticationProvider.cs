using System.Threading.Tasks;

namespace Raet.UM.HAS.Infrastructure.Http.Common
{
    public interface IAuthenticationProvider
    {
        string GetJwt();

        Task<string> GetJwtAsync();
    }
}
