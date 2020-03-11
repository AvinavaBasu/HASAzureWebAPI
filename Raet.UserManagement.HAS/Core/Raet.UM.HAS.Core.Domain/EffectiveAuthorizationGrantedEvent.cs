using System;

namespace Raet.UM.HAS.Core.Domain
{
    public class EffectiveAuthorizationGrantedEvent : EffectiveAuthorizationEvent
    {
        private DateTime _from;

        public DateTime From
        {
            get
            {
                return _from;
            }
            set
            {
                _from = value.ToUniversalTime();
            }
        }
    }
}
