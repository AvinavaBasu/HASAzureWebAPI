using System;
using System.Collections.Generic;
using System.Text;

namespace Raet.UM.HAS.Core.Domain
{
    public class EffectiveIntervalCSVMapper
    {
        public DateTime EffectiveIntervalStart { get; set; }
        public DateTime? EffectiveIntervalEnd { get; set; }
        public bool EffectiveIntervalIsClosed { get; set; }
        public string UserKeyContext { get; set; }
        public string UserKeyId { get; set; }
        public string UserPersonalInfoInitials { get; set; }
        public string UserPersonalInfoLastNameAtBirth { get; set; }
        public string UserPersonalInfoLastNameAtBirthPrefix { get; set; }
        public DateTime UserPersonalInfoBirthDate { get; set; }
        public string TargetKeyContext { get; set; }
        public string TargetKeyId { get; set; }
        public string TargetPersonalInfoInitials { get; set; }
        public string TargetPersonalInfoLastNameAtBirth { get; set; }
        public string TargetPersonalInfoLastNameAtBirthPrefix { get; set; }
        public DateTime TargetPersonalInfoBirthDate { get; set; }
        public string PermissionId { get; set; }
        public string PermissionApplication { get; set; }
        public string PermissionDescription { get; set; }
        public string TenantId { get; set; }
    }
}
