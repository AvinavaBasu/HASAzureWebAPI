using System;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting.Interface;

namespace Raet.UM.HAS.Core.Reporting
{
    public class PermissionRevokedHandler : IEventHandler
    {
        public void ApplyEvent(object @event, EffectiveAuthorizationTimeline aggregate)
        {
            var effectiveAuthorizationRevokedEvent = @event as EffectiveAuthorizationRevokedEvent;
            if (effectiveAuthorizationRevokedEvent == null)
            {
                throw new InvalidOperationException("Unknown event type");
            }
            aggregate.AddPermissionEnd(effectiveAuthorizationRevokedEvent.Until);
        }
    }
}