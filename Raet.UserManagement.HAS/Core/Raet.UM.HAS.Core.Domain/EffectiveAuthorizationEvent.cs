using System;

namespace Raet.UM.HAS.Core.Domain
{
    public abstract class EffectiveAuthorizationEvent
    {
        private DateTime? _dateCreated;

        public EffectiveAuthorization EffectiveAuthorization { get; set; }

        public DateTime? DateCreated
        {
            get
            {
                return _dateCreated;
            }
            set
            {
                _dateCreated = value.HasValue ? value.Value.ToUniversalTime() : (DateTime?)null;
            }
        }
    }
}
