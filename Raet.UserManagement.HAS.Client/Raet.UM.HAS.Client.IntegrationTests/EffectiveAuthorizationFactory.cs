using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Client.IntegrationTests
{
    public class EffectiveAuthorizationFactory
    {
        public static DTOs.EffectiveAuthorization Create(string tenantId)
        {
            return new DTOs.EffectiveAuthorization(tenantId, new DTOs.ExternalId("1", "Youforce.Users"), new DTOs.Permission("1", "Permission", "To do something"), new DTOs.ExternalId("3", "Youforce.Users"));
        }
    }
}
