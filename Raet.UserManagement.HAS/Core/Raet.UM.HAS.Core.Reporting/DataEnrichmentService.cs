using System.Linq;
using System.Threading.Tasks;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting.Interface;

namespace Raet.UM.HAS.Core.Reporting
{
    public class DataEnrichmentService : IDataEnrichmentService
    {
        private readonly IReportingStorage reportingStorage;
        private readonly IEffectiveAuthorizationTimelineFactory effectiveAuthorizationTimelineFactory;
        private readonly IPersonalInfoEnrichmentService personalResolverService;

        public DataEnrichmentService(IEffectiveAuthorizationTimelineFactory effectiveAuthorizationTimelineFactory,
            IPersonalInfoEnrichmentService personalResolverService, IReportingStorage reportingStorage)
        {
            this.effectiveAuthorizationTimelineFactory = effectiveAuthorizationTimelineFactory;
            this.personalResolverService = personalResolverService;
            this.reportingStorage = reportingStorage;
        }

        public async Task AddEffectiveAuthorizationAsync(EffectiveAuthorizationEvent effectiveAuthorizationEvent)
        {
            var effectiveAuthorizationTimeline = await effectiveAuthorizationTimelineFactory.Create(effectiveAuthorizationEvent.EffectiveAuthorization);

            // calculate intervals and early return if intervals
            var intervals = effectiveAuthorizationTimeline.CalculateEffectiveIntervals().ToList();

            if (intervals.Count == 0)
            {
                // Early return if no intervals apply
                return;
            }

            // Get enrichment information for the aggregatre
            var user = await personalResolverService.ResolvePerson(effectiveAuthorizationTimeline.EffectiveAuthorization.User);
            Person targetPerson = null;
            if (effectiveAuthorizationTimeline.EffectiveAuthorization.Target != null)
            {
                targetPerson = await personalResolverService.ResolvePerson(effectiveAuthorizationTimeline.EffectiveAuthorization.Target);
            }

            // Map aggregate to reporting model and save
            var effectiveAuthorizationIntervals = intervals.Select(interval =>
                    new EffectiveAuthorizationInterval(interval, user, targetPerson,
                        effectiveAuthorizationTimeline.EffectiveAuthorization.Permission,
                        effectiveAuthorizationTimeline.EffectiveAuthorization.TenantId))
                .ToList();
            await reportingStorage.SaveAsync(effectiveAuthorizationIntervals);
        }
    }
}
