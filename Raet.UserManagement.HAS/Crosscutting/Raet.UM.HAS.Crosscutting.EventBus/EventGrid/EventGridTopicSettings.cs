using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Crosscutting.EventBus.EventGrid
{
    public class EventGridTopicSettings : IEventGridTopicSettings
    {
        public EventGridTopicSettings(string topicEndpoint, string sasKey)
        {
            TopicEndpoint = topicEndpoint;
            SasKey = sasKey;
        }
        public string TopicEndpoint { get; set; }

        public string SasKey { get; set; }

        public bool IsValid => (!string.IsNullOrEmpty(TopicEndpoint) && !string.IsNullOrEmpty(SasKey));
    }
}
