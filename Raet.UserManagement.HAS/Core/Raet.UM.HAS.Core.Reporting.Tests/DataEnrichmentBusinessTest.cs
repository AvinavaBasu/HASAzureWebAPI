using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Core.Reporting.Business;
using Raet.UM.HAS.Core.Reporting.Tests.Helper;
using Xunit;

namespace Raet.UM.HAS.Core.Reporting.Tests
{
    public class DataEnrichmentBusinessTest
    {
        private Mock<IUserInformation> _userInformation;
        private Mock<IReportingStorage> _reportingStorage;
        private Mock<IReadRawEventStorage> _readRawEventStorage;
        private IEffectiveAuthorizationTimelineFactory _effectiveAuthorizationTimelineFactory;
        private EffectiveAuthorizationHandlerFactory _effectiveAuthorizationHandlerFactory;
        private DataEnrichmentBusiness _dataEnrichmentBusiness;
        private TestData _testData;

        public DataEnrichmentBusinessTest()
        {
            _testData = FileReader.Get<TestData>("EventGridTestData.json");
            _userInformation = new Mock<IUserInformation>();
            _reportingStorage = new Mock<IReportingStorage>();
            _readRawEventStorage = new Mock<IReadRawEventStorage>();
            _effectiveAuthorizationHandlerFactory = new EffectiveAuthorizationHandlerFactory();

            // Register handlers
            _effectiveAuthorizationHandlerFactory.RegisterHandler(typeof(EffectiveAuthorizationGrantedEvent),
                new PermissionGrantedHandler());
            _effectiveAuthorizationHandlerFactory.RegisterHandler(typeof(EffectiveAuthorizationRevokedEvent),
                new PermissionRevokedHandler());
        }

        [Fact]
        public void When_MultipleGrantAndRevoked_EventExists_Then_EnrichedData_ShouldBeValid()
        {
            GrantRevokeEventTest(_testData.ValidGrantedEventData, grantEventCount: 3, revokeEventCount: 2);
            Assert.True(_dataEnrichmentBusiness.EnrichedData.Count == 2);
            Assert.NotNull(_dataEnrichmentBusiness.UpsertId);
        }

        [Fact]
        public void When_MultipleRevokeEventOnlyExists_Then_EnrichedData_ShouldBeEmpty()
        {
            GrantRevokeEventTest(_testData.ValidRevokedEventData, grantEventCount: 0, revokeEventCount: 5);
            Assert.Null(_dataEnrichmentBusiness.EnrichedData);
            Assert.Null(_dataEnrichmentBusiness.UpsertId);
        }


        private void GrantRevokeEventTest(object eventData, int grantEventCount, int revokeEventCount)
        {
            // set the users
            var externalIds = GetExternalIds(eventData.ToString());
            var users = Generator.GeneratePersonsData(externalIds);
            _userInformation.Setup(e => e.GetUsers(It.IsAny<IEnumerable<ExternalId>>()))
                .Returns(users);

            // mock reporting storage
            _reportingStorage.Setup(e => e.SaveAsync(It.IsAny<List<EffectiveAuthorizationInterval>>()))
                .Returns(Task.FromResult("123DAJSK"));

            // mock IReadRawEventStorage
            _readRawEventStorage.Setup(e => e.GetRawEventsAsync(It.IsAny<EffectiveAuthorization>()))
                .Returns(Task.FromResult(Generator.GenerateReadRawEventData(grantEventCount, revokeEventCount)));

            // set IEffectiveAuthorizationTimelineFactory
            _effectiveAuthorizationTimelineFactory =
                new EffectiveAuthorizationTimelineFactory(_readRawEventStorage.Object,
                    _effectiveAuthorizationHandlerFactory);


            _dataEnrichmentBusiness = new DataEnrichmentBusiness(_effectiveAuthorizationTimelineFactory,
                _reportingStorage.Object, _userInformation.Object, new Mock<ILogger>().Object);
            _dataEnrichmentBusiness.Process(JsonConvert.SerializeObject(_testData.ValidGrantedEventData));
        }

        private IEnumerable<ExternalId> GetExternalIds(string eventTriggerData)
        {
            var eventData = GridMessageToEvents.Convertor(eventTriggerData); 
            return new List<ExternalId>()
            {
                eventData.EffectiveAuthorization.User,
                eventData.EffectiveAuthorization.Target
            };
        }
    }
}

