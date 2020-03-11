using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Client.DTOs
{

    public class EffectiveAuthorizationRevokedEvent : EffectiveAuthorizationEvent
    {
        public override string Action => "revoked";
        public DateTime Until { get; set; }
    }
}
