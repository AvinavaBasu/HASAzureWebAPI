using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Client.DTOs
{
    public class EffectiveAuthorizationGrantedEvent : EffectiveAuthorizationEvent
    {
        public override string Action => "granted";
        public DateTime From { get; set; }

    }
}
