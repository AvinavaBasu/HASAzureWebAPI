using System.Collections.Generic;

namespace Raet.UM.HAS.Core.Domain
{
    public class EffectiveAuthorizationInterval
    {
        public EffectiveAuthorizationInterval(Interval effectiveInterval, Person user, Person targetPerson, Permission permission, string tenantId)
        {
            EffectiveInterval = effectiveInterval;
            User = user;
            TargetPerson = targetPerson;
            Permission = permission;
            TenantId = tenantId;
        }

        public Interval EffectiveInterval { get; }
        public Person User { get; }
        public Person TargetPerson { get; }

        public Permission Permission { get; }

        public string TenantId { get; }
    }
}