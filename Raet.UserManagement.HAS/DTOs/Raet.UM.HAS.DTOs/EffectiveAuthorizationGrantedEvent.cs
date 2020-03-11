using System;
using System.ComponentModel.DataAnnotations;

namespace Raet.UM.HAS.DTOs
{
    public class EffectiveAuthorizationGrantedEvent : EffectiveAuthorizationEvent
    {
        private DateTime? _fromDateTime;

        [Required]
        
        public DateTime? FromDateTime
        {
            get
            {
                return _fromDateTime;
            }
            set
            {
                _fromDateTime = value.HasValue ? value.Value.ToUniversalTime() : (DateTime?)null;
            }
        }

    }
}
