using System.Collections.Generic;
using Newtonsoft.Json;
using Raet.UM.HAS.Core.Domain;

namespace Raet.UM.HAS.Core.Reporting.Tests
{
    public class TestData
    {
        public object ValidRevokedEventData { get; set; }
        public object ValidGrantedEventData { get; set; }
        public object InValidEventData { get; set; }
        public List<object> ReadRawEventData { get; set; }

        public List<EffectiveAuthorizationEvent> GetReadRawEventData()
        {
            var res = new List<EffectiveAuthorizationEvent>();
            foreach (var data in ReadRawEventData)
            {
                if (data.ToString().Contains("granted"))
                    res.Add(JsonConvert.DeserializeObject<EffectiveAuthorizationGrantedEvent>(data.ToString()));
                else
                    res.Add(JsonConvert.DeserializeObject<EffectiveAuthorizationRevokedEvent>(data.ToString()));
            }

            return res;
        }
    }
}
    