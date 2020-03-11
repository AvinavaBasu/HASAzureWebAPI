using System;
using System.Threading.Tasks;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Crosscutting.EventBus;

namespace Raet.UM.HAS.Core.Application
{
    public class ReactiveEffectiveAuthorizationLogging : IEffectiveAuthorizationLogging
    {
        private readonly IWriteRawEventStorage rawEventStorage;

        private readonly IEventBus bus;

        public ReactiveEffectiveAuthorizationLogging(IWriteRawEventStorage rawEventStorage, IEventBus bus)
        {
            this.rawEventStorage = rawEventStorage;
            this.bus = bus;
        }

        public async Task<string> AddAuthLogAsync(EffectiveAuthorizationEvent effectiveAuthorizationEvent)
        {
            // Store event and 
            var output = await rawEventStorage.WriteRawEventAsync(effectiveAuthorizationEvent);
            // Re-raise event using EventGrid
            await bus.GetTopicEffectiveAuthorizationEventStored().DispatchAsync(effectiveAuthorizationEvent);

            return output;
        }
    }
}
