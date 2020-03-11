using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Core.Reporting.Business
{
    public class EAAggregateBusiness : IEAAggregateBusiness
    {
        private IReportingStorage _reportingStorage { get; set; }
        public EAAggregateBusiness(IReportingStorage reportingStorage)
        {
            _reportingStorage = reportingStorage;
        }

        public async Task<IList<string>> GetApplication(string tenantId)
        {
            var applications = await _reportingStorage.GetApplication(tenantId);
            return applications;
        }
        public async Task<IList<PermissionDto>> GetPermissionData(string application, string tenantId)
        {
            var permissions = await _reportingStorage.FetchPermissionData(application,tenantId);
            IList<PermissionDto> result = new List<PermissionDto>();

            foreach (var permission in permissions)
            {
                result.Add(new PermissionDto {
                    Id = permission.Id,
                    Application = permission.Application,
                    Description = permission.Description
                });
            }

            return result;
        }

        public async Task<IList<CustomUser>> GetUsers(IList<string> permissions, string userType, string application,string tenantId)
        {
            var users = await _reportingStorage.GetUsers(permissions, userType,application,tenantId);
            var customUsersList = new List<CustomUser>();
            try
            {
                foreach (var item in users)
                {
                    var user = new CustomUser
                    {
                        Key = item.Key,
                        UserName = item.PersonalInfo.LastNameAtBirth?.Trim() + ","
                      + item.PersonalInfo.LastNameAtBirthPrefix?.Trim() + " " + item.PersonalInfo.Initials?.Trim() + " " +
                      " (" + item.PersonalInfo.BirthDate.ToString("dd/MM/yyyy").Trim() + ")"
                    };
                    customUsersList.Add(user);
                }
                return customUsersList;
            }
            catch (System.Exception ex)
            {
                throw ex.InnerException;
            }
            
        }

    }
}
