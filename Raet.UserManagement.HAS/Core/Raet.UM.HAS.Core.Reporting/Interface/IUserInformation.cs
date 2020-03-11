using Raet.UM.HAS.Core.Domain;
using System.Collections.Generic;

namespace Raet.UM.HAS.Core.Reporting.Interface
{
    public interface IUserInformation
    {
        IEnumerable<Person> GetUsers(IEnumerable<ExternalId> userIds);
    }
}
