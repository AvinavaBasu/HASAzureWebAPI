using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Raet.UM.HAS.Crosscutting.EventBus.EventGrid
{
    public class EventGridBus:IEventBus
    {
        private readonly IDictionary<string, EventGridTopicSettings> _topicsSettings;
        private readonly ConcurrentDictionary<string, object> _topics = new ConcurrentDictionary<string, object>();

        public EventGridBus( IDictionary<string, EventGridTopicSettings> topics )
        {
            _topicsSettings = topics;
        }

        
        public ITopic<T> GetTopic<T>(string name)
        {
            if (_topicsSettings.TryGetValue(name, out var config))
            {
                var topic=_topics.GetOrAdd(name, new AzureEventGridTopic<T>(config));
                if (topic is ITopic<T> result)
                    return result;
            }

            throw new Exception("Invalid topic type");
        }

    }
}
