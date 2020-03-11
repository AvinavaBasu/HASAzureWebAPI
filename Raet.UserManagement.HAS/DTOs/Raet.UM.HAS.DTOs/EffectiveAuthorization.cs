using System;
using System.ComponentModel.DataAnnotations;

namespace Raet.UM.HAS.DTOs
{
    public class EffectiveAuthorization
    {
        [Required(AllowEmptyStrings = false)]
        public string TenantId { get; set; }

        [Required]
        public ExternalId User { get; set; }

        [Required]
        public Permission Permission { get; set; }

        public ExternalId Target { get; set; }

        public EffectiveAuthorization()
        {

        }

        public EffectiveAuthorization(string tenantId, ExternalId user, Permission permission, ExternalId target = null)
        {
            TenantId = tenantId;
            User = user;
            Permission = permission;
            Target = target;
        }

        public override bool Equals(object obj)
        {
            var effectiveAuthorization = obj as EffectiveAuthorization;
            return effectiveAuthorization != null && Equals(effectiveAuthorization);
        }

        protected bool Equals(EffectiveAuthorization other)
        {
            return User.Equals(other.User) &&
                Permission.Equals(other.Permission) &&
                (Target?.Equals(other.Target) ?? other.Target == null);
        }

        public override int GetHashCode()
        {
            return Target != null ?
                Tuple.Create(User, Permission, Target).GetHashCode() :
                Tuple.Create(User, Permission).GetHashCode();
        }
    }
}
