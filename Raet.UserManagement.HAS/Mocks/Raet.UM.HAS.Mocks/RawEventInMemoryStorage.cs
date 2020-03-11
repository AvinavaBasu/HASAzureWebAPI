using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raet.UM.HAS.Core.Application;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting;
using Raet.UM.HAS.Core.Reporting.Interface;


namespace Raet.UM.HAS.Mocks
{
    public class RawEventInMemoryStorage : IWriteRawEventStorage,IReadRawEventStorage
    {
        private readonly List<EffectiveAuthorizationEvent> events = new List<EffectiveAuthorizationEvent>();

        public Task<string> WriteRawEventAsync(EffectiveAuthorizationEvent effectiveAuthorizationEvent)
        {
            events.Add(effectiveAuthorizationEvent);
            return Task.FromResult<string>(events.IndexOf(effectiveAuthorizationEvent).ToString());
        }

        public Task<List<EffectiveAuthorizationEvent>> GetRawEventsAsync(EffectiveAuthorization effectiveAuthorization)
        {
            var matchedEvents = events.Where(e => e.EffectiveAuthorization.Equals(effectiveAuthorization)).ToList();
            return Task.FromResult(matchedEvents);
        }
    }
}