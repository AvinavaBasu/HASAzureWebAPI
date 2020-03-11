using System.Threading.Tasks;
using Raet.UM.HAS.Core.Domain;

namespace Raet.UM.HAS.Core.Reporting.Interface
{
    public interface IEffectiveAuthorizationTimelineFactory
    {
        Task<EffectiveAuthorizationTimeline> Create(EffectiveAuthorization effectiveAuthorization);
    }
}