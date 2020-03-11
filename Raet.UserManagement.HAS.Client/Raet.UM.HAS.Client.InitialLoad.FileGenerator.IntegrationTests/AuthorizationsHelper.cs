using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raet.UM.HAS.DTOs;
using Raet.UM.HAS.Client.IntegrationTests;

namespace Raet.UM.HAS.Client.InitialLoad.FileGenerator.IntegrationTests
{
    public class AuthorizationsHelper
    {
        public static List<Authorization> GetAuthorizations(int authorizationNumber, string tenantId, DateTime from)
        {
            var authorizations = new List<Authorization>();

            for (int i = 0; i < authorizationNumber; i++)
            {
                authorizations.Add(new Authorization() { EffectiveAuthorization = EffectiveAuthorizationFactory.Create(tenantId), From = from });
            }

            return authorizations;
        }
    }
}
