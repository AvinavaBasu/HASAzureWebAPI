using Raet.UM.HAS.Core.Domain;

namespace Raet.UM.HAS.Core.Reporting.Interface
{
    public interface IEventHandler
    {
        void ApplyEvent(object @event, EffectiveAuthorizationTimeline aggregate);
    }
}