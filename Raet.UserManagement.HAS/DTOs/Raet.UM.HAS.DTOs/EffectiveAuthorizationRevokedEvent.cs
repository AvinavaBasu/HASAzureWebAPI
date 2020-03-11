using System;
using System.ComponentModel.DataAnnotations;

namespace Raet.UM.HAS.DTOs
{

    public class EffectiveAuthorizationRevokedEvent : EffectiveAuthorizationEvent
    {
        private DateTime? _untilDateTime;

        [Required]
        public DateTime? UntilDateTime
        {
            get
            {
                return _untilDateTime;
            }
            set
            {
                _untilDateTime = value.HasValue ? value.Value.ToUniversalTime() : (DateTime?)null;
            }
        }
    }
}
