using System;
using System.ComponentModel.DataAnnotations;

namespace Raet.UM.HAS.Client.DTOs
{
    public class EffectiveAuthorization
    {
        [Required(AllowEmptyStrings = false)]
        public string TenantId { get; private set; }

        [Required]
        public ExternalId User { get; private set; }

        [Required]
        public Permission Permission { get; private set; }

        public ExternalId TargetPerson { get; private set; }

        public EffectiveAuthorization(string tenantId, ExternalId user, Permission permission, ExternalId targetPerson = null)
        {
            TenantId = tenantId;
            User = user;
            Permission = permission;
            TargetPerson = targetPerson;
        }
    }
}
