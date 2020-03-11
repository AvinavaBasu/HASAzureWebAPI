using Raet.UM.HAS.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Raet.UM.HAS.Core.Reporting.Interface
{
    public interface IEAAggregateBusiness
    {

        Task<IList<DTOs.PermissionDto>> GetPermissionData(string application, string tenantId);
        Task<IList<CustomUser>> GetUsers(IList<string> permissions, string userType,string application, string tenantId);
        Task<IList<string>> GetApplication(string tenantId);
    }
}
