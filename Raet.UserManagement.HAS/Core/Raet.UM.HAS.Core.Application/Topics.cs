using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Crosscutting.EventBus;

namespace Raet.UM.HAS.Core.Application
{
    public static class Topics
    {

        public const string EFFECTIVE_AUTHORIZATION_EVENT_STORED = "EFFECTIVE_AUTHORIZATION_EVENT_STORED";

        public static ITopic<EffectiveAuthorizationEvent> GetTopicEffectiveAuthorizationEventStored(this IEventBus eventBus)
        {
            return eventBus.GetTopic<EffectiveAuthorizationEvent>(EFFECTIVE_AUTHORIZATION_EVENT_STORED);
        }
    }
}
