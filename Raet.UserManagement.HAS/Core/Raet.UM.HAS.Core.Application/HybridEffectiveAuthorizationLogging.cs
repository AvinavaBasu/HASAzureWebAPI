using System.Threading.Tasks;
using Raet.UM.HAS.Core.Domain;

using Raet.UM.HAS.Crosscutting.EventBus;

namespace Raet.UM.HAS.Core.Application
{
    public class HybridEffectiveAuthorizationLogging : IEffectiveAuthorizationLogging
    {
        private readonly IWriteRawEventStorage rawEventStorage;

        private readonly IEventBus bus;

        public HybridEffectiveAuthorizationLogging(IWriteRawEventStorage rawEventStorage, 
            IEventBus bus)
        {
            this.rawEventStorage = rawEventStorage;
            this.bus = bus;
        }

        public async Task<string> AddAuthLogAsync(EffectiveAuthorizationEvent effectiveAuthorizationEvent)
        {
            // Store event and re-raise event
            var output = await rawEventStorage.WriteRawEventAsync(effectiveAuthorizationEvent);

            await bus.GetTopicEffectiveAuthorizationEventStored().
                DispatchAsync(effectiveAuthorizationEvent);

            return output;
        }
    }
}
