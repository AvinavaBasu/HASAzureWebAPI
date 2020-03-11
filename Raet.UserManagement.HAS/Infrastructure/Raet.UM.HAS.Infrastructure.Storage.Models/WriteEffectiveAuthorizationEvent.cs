using System;

namespace Raet.UM.HAS.Infrastructure.Storage.Models
{
    public class WriteEffectiveAuthorizationEvent
    {
        public string Action { get; set; }

        public EffectiveAuthorization EffectiveAuthorization { get; set; }

        public DateTime DateCreated
        {
            get; set;
        }
    }
}
