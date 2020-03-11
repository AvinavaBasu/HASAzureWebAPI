using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Raet.UM.HAS.DTOs
{
    public class ReportingEvent
    {
        [Required]
        public string Application { get; set; }
        [Required]
        public IList<string> Permissions { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string TenantId { get; set; }

        public ExternalId SourceUser { get; set; }

        public ExternalId TargetUser { get; set; }

    }
}
