using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting.Interface;



namespace Raet.UM.HAS.Mocks
{
    public class ReportingInMemoryStorage : IReportingStorage
    {
        private readonly Dictionary<EffectiveAuthorization, IList<EffectiveAuthorizationInterval>> effectiveAuthorizationIntervalStore = 
            new Dictionary<EffectiveAuthorization, IList<EffectiveAuthorizationInterval>>();

        public Task<string> SaveAsync(IList<EffectiveAuthorizationInterval> effectiveAuthorizationIntervals)
        {
            if (effectiveAuthorizationIntervals.Count == 0)
            {
                return Task.FromResult<string>(string.Empty);
            }

            // Asumming all intervals belong to same effective authorization (it is a mock!)
            var effectiveAuthorizationIntervalRef = effectiveAuthorizationIntervals[0];
            var key = new EffectiveAuthorization
            {
                User = effectiveAuthorizationIntervalRef.User.Key,
                Target = effectiveAuthorizationIntervalRef.TargetPerson?.Key,
                Permission = effectiveAuthorizationIntervalRef.Permission,
                TenantId = effectiveAuthorizationIntervalRef.TenantId
            };
            effectiveAuthorizationIntervalStore.Remove(key);

            effectiveAuthorizationIntervalStore.Add(key, effectiveAuthorizationIntervals);
            return Task.FromResult<string>(string.Empty);
        }

        public Task<IList<EffectiveAuthorizationInterval>> GetIntervals(EffectiveAuthorization effectiveAuthorization)
        {
            effectiveAuthorizationIntervalStore.TryGetValue(effectiveAuthorization,
                out IList<EffectiveAuthorizationInterval> intervals);
            return Task.FromResult(intervals);
        }

        public Task<IList<Permission>> FetchPermissionData(IList<string> tenantIds)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Person>> GetUsers(IList<string> permissions, string userType,string application, string tenantId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetApplication(string tenantId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<EffectiveAuthorizationInterval>> FetchReportingData(ReportingEvent ReportingEvent)
        {
            throw new NotImplementedException();
        }

        Task<IList<Permission>> IReportingStorage.FetchPermissionData(string application, string tenantId)
        {
            throw new NotImplementedException();
        }
    }
}