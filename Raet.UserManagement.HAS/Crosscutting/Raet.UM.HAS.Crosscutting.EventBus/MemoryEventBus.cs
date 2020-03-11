using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Raet.UM.HAS.Core.Domain;

namespace Raet.UM.HAS.Crosscutting.EventBus
{
    public class MemoryEventBus: IEventBus
    {
        private readonly ConcurrentDictionary<string, object> _topics = new ConcurrentDictionary<string, object>();

        public ITopic<T> GetTopic<T>(string name)
        {
            var topic = _topics.GetOrAdd(name, new HotObservableTopic<T>());
            if (topic is ITopic<T> result)
                return result;

            throw new Exception("Invalid topic type");
        }

    }
}
