using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Client.DTOs
{
    public abstract class EffectiveAuthorizationEvent
    {
        public abstract string Action { get; }

        public EffectiveAuthorization Auth { get; set; }
    }
}
