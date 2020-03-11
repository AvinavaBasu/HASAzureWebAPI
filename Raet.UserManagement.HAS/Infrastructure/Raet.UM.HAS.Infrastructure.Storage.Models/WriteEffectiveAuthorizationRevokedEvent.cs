using System;

namespace Raet.UM.HAS.Infrastructure.Storage.Models
{
    public class WriteEffectiveAuthorizationRevokedEvent : WriteEffectiveAuthorizationEvent
    {
        public DateTime? Until { get; set; }
    }
}
