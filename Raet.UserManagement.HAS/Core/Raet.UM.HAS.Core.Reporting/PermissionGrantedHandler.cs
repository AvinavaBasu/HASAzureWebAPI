using System;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting.Interface;

namespace Raet.UM.HAS.Core.Reporting
{
    public class PermissionGrantedHandler : IEventHandler
    {
        public void ApplyEvent(object @event, EffectiveAuthorizationTimeline aggregate)
        {
            var effectiveAuthorizationGrantedEvent = @event as EffectiveAuthorizationGrantedEvent;
            if (effectiveAuthorizationGrantedEvent == null)
            {
                throw new InvalidOperationException("Unknown event type");
            }
            aggregate.AddPermissionStart(effectiveAuthorizationGrantedEvent.From);
        }
    }
}