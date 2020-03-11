using System;

namespace Raet.UM.HAS.Core.Domain
{
    public class EffectiveAuthorizationRevokedEvent: EffectiveAuthorizationEvent
    {

        private DateTime _until;

        public DateTime Until
        {
            get
            {
                return _until;
            }
            set
            {
                _until = value.ToUniversalTime();
            }
        }
    }
}
