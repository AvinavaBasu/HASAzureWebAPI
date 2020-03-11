using System.Threading.Tasks;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting.Interface;

namespace Raet.UM.HAS.Core.Reporting
{
    public class EffectiveAuthorizationTimelineFactory : IEffectiveAuthorizationTimelineFactory
    {
        private readonly IReadRawEventStorage eventStore;
        private readonly IEffectiveAuthorizationHandlerFactory eventHandlerFactory;

        public EffectiveAuthorizationTimelineFactory(IReadRawEventStorage eventStore,
            IEffectiveAuthorizationHandlerFactory eventHandlerFactory)
        {
            this.eventStore = eventStore;
            this.eventHandlerFactory = eventHandlerFactory;
        }

        public async Task<EffectiveAuthorizationTimeline> Create(EffectiveAuthorization effectiveAuthorization)
        {
            var events = await eventStore.GetRawEventsAsync(effectiveAuthorization);

            var effectiveAuthorizationTimeline = new EffectiveAuthorizationTimeline(effectiveAuthorization);
            foreach (var authorizationEvent in events)
            {
                eventHandlerFactory.GetHandler(authorizationEvent)
                    .ApplyEvent(authorizationEvent, effectiveAuthorizationTimeline);
            }
            
            return effectiveAuthorizationTimeline;
        }

    }
}