using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Raet.UM.HAS.Core.Domain;


namespace Raet.UM.HAS.Core.Reporting.Interface
{
    public interface IReportingStorage
    {
        Task<string> SaveAsync(IList<EffectiveAuthorizationInterval> effectiveAuthorizationIntervals);
        Task<IList<EffectiveAuthorizationInterval>> GetIntervals(EffectiveAuthorization effectiveAuthorization);
        Task<IList<EffectiveAuthorizationInterval>> FetchReportingData(ReportingEvent ReportingEvent);
        Task<IList<Permission>> FetchPermissionData(string application, string tenantId);
        Task<IList<Person>> GetUsers(IList<string> permissions, string userType, string application, string tenantId);
        Task<IList<string>> GetApplication(string tenantId);
    }
}