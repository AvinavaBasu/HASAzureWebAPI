using System;

namespace Raet.UM.HAS.Infrastructure.Storage.Models
{
    public class WriteEffectiveAuthorizationGrantedEvent : WriteEffectiveAuthorizationEvent
    {
        public DateTime? From { get; set; }
    }
}
