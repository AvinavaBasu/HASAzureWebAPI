using Raet.UM.HAS.Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Raet.UM.HAS.Core.Reporting.Helper
{
    public static class Extension
    {
        public static IEnumerable<ExternalId> GetUserIds(this EffectiveAuthorizationEvent input)
        {
            var result = new List<ExternalId>
            {
                input.EffectiveAuthorization.User,
            };

            if (input.EffectiveAuthorization.Target != null)
                result.Add(input.EffectiveAuthorization.Target);

            return result;
        }
    }
}
