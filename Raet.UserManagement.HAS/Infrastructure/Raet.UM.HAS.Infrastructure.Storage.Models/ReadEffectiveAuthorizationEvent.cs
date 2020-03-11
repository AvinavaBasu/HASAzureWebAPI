using System;

namespace Raet.UM.HAS.Infrastructure.Storage.Models
{
    public class ReadEffectiveAuthorizationEvent
    {
        public string id { get; set; }

        public DateTime? From { get; set; }

        public DateTime? Until { get; set; }

        public string Action { get; set; }

        public EffectiveAuthorization EffectiveAuthorization { get; set; }

        public DateTime DateCreated
        {
            get; set;
        }
    }
}
