using Raet.UM.HAS.Core.Domain;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Core.Application
{
    public interface IEffectiveAuthorizationLogging
    {
        Task<string> AddAuthLogAsync(EffectiveAuthorizationEvent effectiveAuthorizationEvent); 
    }
}
