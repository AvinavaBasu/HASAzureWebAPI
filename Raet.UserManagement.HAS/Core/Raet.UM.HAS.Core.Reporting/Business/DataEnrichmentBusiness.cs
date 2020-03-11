using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Core.Reporting.Helper;
using Raet.UM.HAS.Crosscutting.Exceptions;

namespace Raet.UM.HAS.Core.Reporting.Business
{
    public class DataEnrichmentBusiness : IDataEnrichmentBusiness
    {
        private readonly IEffectiveAuthorizationTimelineFactory _effectiveAuthorizationTimelineFactory;
        private readonly IUserInformation _userInformation;
        private readonly IReportingStorage _reportingStorage;
        private readonly ILogger _logger;
        public IList<EffectiveAuthorizationInterval> EnrichedData { get; private set; }
        public string UpsertId { get; private set; }

        public DataEnrichmentBusiness(IEffectiveAuthorizationTimelineFactory effectiveAuthorizationTimelineFactory,
            IReportingStorage reportingStorage, IUserInformation userInformation, ILogger logger
        )
        {
            _effectiveAuthorizationTimelineFactory = effectiveAuthorizationTimelineFactory;
            _reportingStorage = reportingStorage;
            _userInformation = userInformation;
            _logger = logger;
        }

        public async void Process(string eventData)
        {
            try
            {
                _logger.LogInformation("GridMessageToEvents Started");
                var effectiveAuthorizationEvent = GridMessageToEvents.Convertor(eventData);

                _logger.LogInformation("EffectiveAuthorizationTimelineFactory process started");
                var effectiveAuthorizationTimeline =
                    await _effectiveAuthorizationTimelineFactory.Create(effectiveAuthorizationEvent
                        .EffectiveAuthorization);

                _logger.LogInformation("Calculate Effective Intervals started");
                var intervals = effectiveAuthorizationTimeline.CalculateEffectiveIntervals().ToList();

                if (intervals.Count == 0)
                {
                    _logger.LogInformation("No new intervals were found for this event");
                    return;
                }

                _logger.LogInformation("Fetching user information");
                var users = _userInformation.GetUsers(effectiveAuthorizationEvent.GetUserIds()).ToList();
                var source = users[0];
                var target = users.Count == 2 ? users[1] : null;

                _logger.LogInformation($"Intervals count : {intervals.Count}");
                EnrichedData = intervals.Select(interval =>
                        new EffectiveAuthorizationInterval(interval, source, target,
                            effectiveAuthorizationTimeline.EffectiveAuthorization.Permission,
                            effectiveAuthorizationTimeline.EffectiveAuthorization.TenantId))
                    .ToList();

                _logger.LogInformation($"saving enriched data");
                await SaveEnrichedData();
            }
            catch (InvalidEventDataException ex)
            {
                _logger.LogError(ex, "Incorrect EventData format");
            }
            catch (RawEventStorageException ex)
            {
                _logger.LogError(ex.ToString());
            }
            catch (StorageException ex)
            {
                _logger.LogError(ex.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        private async Task SaveEnrichedData()
        {
            UpsertId = await _reportingStorage.SaveAsync(EnrichedData);
        }
    }
}
