using Newtonsoft.Json;
using Raet.UM.HAS.Core.Domain;

namespace Raet.UM.HAS.Core.Reporting.Business
{

    public static class GridMessageToEvents
    {
        public static EffectiveAuthorizationEvent Convertor(string eventGridMessage)
        {
            EffectiveAuthorizationEvent eaEvent = null;
            if (eventGridMessage.Contains("From"))
            {
                eaEvent = JsonConvert.DeserializeObject<EffectiveAuthorizationGrantedEvent>(eventGridMessage);
            }
            else if (eventGridMessage.Contains("Until"))
            {
                eaEvent = JsonConvert.DeserializeObject<EffectiveAuthorizationRevokedEvent>(eventGridMessage);
            }

            return eaEvent;
        }
    }
}
