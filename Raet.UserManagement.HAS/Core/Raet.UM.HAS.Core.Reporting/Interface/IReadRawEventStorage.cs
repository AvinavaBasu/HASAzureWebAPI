using System.Collections.Generic;
using System.Threading.Tasks;
using Raet.UM.HAS.Core.Domain;

namespace Raet.UM.HAS.Core.Reporting.Interface
{
    public interface IReadRawEventStorage
    {
        Task<List<EffectiveAuthorizationEvent>> GetRawEventsAsync(EffectiveAuthorization effectiveAuthorization);
    }
}
