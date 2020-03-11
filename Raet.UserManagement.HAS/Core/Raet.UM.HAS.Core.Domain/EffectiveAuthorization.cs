using System;

namespace Raet.UM.HAS.Core.Domain
{
    public class EffectiveAuthorization
    {
        public string TenantId { get; set; }

        public ExternalId User { get; set; }

        public Permission Permission { get; set; }

        public ExternalId Target { get; set; }

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
