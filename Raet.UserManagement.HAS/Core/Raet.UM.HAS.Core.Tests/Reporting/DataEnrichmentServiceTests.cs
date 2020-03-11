using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raet.UM.HAS.Core.Reporting;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Mocks;
using Moq;
using System.Threading.Tasks;
using Raet.UM.HAS.Core.Reporting.Interface;

namespace Raet.UM.HAS.Core.Tests.Reporting
{
    [TestClass]
    public class DataEnrichmentServiceTests
    {
        private readonly string _tenantId = "Test001";

        private readonly string _defaultId = "IC000001";
        private readonly string _defaultContext = "Youforce";

        private readonly string _defaultApplication = "YOUFORCE";
        private readonly string _defaultPermissionId = "HomeAccess";
        private readonly string _defaultDescription = "Home Acess";

        [TestMethod]
        public async Task AddEffectiveAuthorizationAsync_Event_Stored_With_Valid_Authorization_Without_Target_Calls_IReportingStorage_SaveAsyc()
        {
            //Arrange
            var eaEventsStorate = new RawEventInMemoryStorage();

            var eaHandlerFactory = new EffectiveAuthorizationHandlerFactory();
            eaHandlerFactory.RegisterHandler(typeof(EffectiveAuthorizationGrantedEvent), new PermissionGrantedHandler());

            var eatimelineFactory = new EffectiveAuthorizationTimelineFactory(eaEventsStorate, eaHandlerFactory);

            var prService = new PersonalInfoEnrichmentService(new MockPersonalLocalStorage(), new MockPersonalInfoExternalServiceFactory(), new MockInMemoryLogger());

            var rpStorageMock = new Mock<IReportingStorage>();
            var owner = new ExternalId() { Id = _defaultId, Context = _defaultContext };
            var permission = new Permission() { Application = _defaultApplication, Id = _defaultPermissionId, Description = _defaultDescription };
            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = _tenantId, Permission = permission, User = owner };
            var eaEvent = new EffectiveAuthorizationGrantedEvent() { From = new DateTime(2018, 1, 1), EffectiveAuthorization = effectiveAuthorization };

            await eaEventsStorate.WriteRawEventAsync(eaEvent); //event needs to be previously writteng to storage.

            var dataEnrichmentService = new DataEnrichmentService(eatimelineFactory, prService, rpStorageMock.Object);

            //Act
            await dataEnrichmentService.AddEffectiveAuthorizationAsync(eaEvent);

            //Assert
            rpStorageMock.Verify(v => v.SaveAsync(It.IsAny<IList<EffectiveAuthorizationInterval>>()));
        }

        [TestMethod]
        public async Task AddEffectiveAuthorizationAsync_Event_Stored_With_Valid_Authorization_Without_Target_Stores_Correct_Information()
        {
            //Arrange
            var eaEventsStorate = new RawEventInMemoryStorage();

            var eaHandlerFactory = new EffectiveAuthorizationHandlerFactory();
            eaHandlerFactory.RegisterHandler(typeof(EffectiveAuthorizationGrantedEvent), new PermissionGrantedHandler());

            var eatimelineFactory = new EffectiveAuthorizationTimelineFactory(eaEventsStorate, eaHandlerFactory);

            var prService = new PersonalInfoEnrichmentService(new MockPersonalLocalStorage(), new MockPersonalInfoExternalServiceFactory(), new MockInMemoryLogger());

            var rpStorage = new ReportingInMemoryStorage();
            var owner = new ExternalId() { Id = _defaultId, Context = _defaultContext };
            var permission = new Permission() { Application = _defaultApplication, Id = _defaultPermissionId, Description = _defaultDescription };
            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = _tenantId, Permission = permission, User = owner };
            var eaEvent = new EffectiveAuthorizationGrantedEvent() { From = new DateTime(2018, 1, 1), EffectiveAuthorization = effectiveAuthorization };

            await eaEventsStorate.WriteRawEventAsync(eaEvent); //event needs to be previously writteng to storage.

            var dataEnrichmentService = new DataEnrichmentService(eatimelineFactory, prService, rpStorage);

            //Act
            await dataEnrichmentService.AddEffectiveAuthorizationAsync(eaEvent);

            var intervals = await rpStorage.GetIntervals(effectiveAuthorization);
            
            //Assert
            Assert.AreEqual(1, intervals.Count);
            Assert.AreEqual(_tenantId, intervals[0].TenantId);
            Assert.AreEqual(owner, intervals[0].User.Key);
            Assert.AreEqual(permission, intervals[0].Permission);
            Assert.AreEqual(null, intervals[0].TargetPerson);
        }

        [TestMethod]
        public async Task AddEffectiveAuthorizationAsync_Event_Stored_With_Invalid_Authorization_No_Owner_Throws_NullReferenceException()
        {
            //Arrange
            var eaEventsStorate = new RawEventInMemoryStorage();

            var eaHandlerFactory = new EffectiveAuthorizationHandlerFactory();
            eaHandlerFactory.RegisterHandler(typeof(EffectiveAuthorizationGrantedEvent), new PermissionGrantedHandler());

            var eatimelineFactory = new EffectiveAuthorizationTimelineFactory(eaEventsStorate, eaHandlerFactory);

            var prService = new PersonalInfoEnrichmentService(new MockPersonalLocalStorage(), new MockPersonalInfoExternalServiceFactory(), new MockInMemoryLogger());
            var rpStorage = new ReportingInMemoryStorage();
            var owner = new ExternalId() { Id = _defaultId, Context = _defaultContext };
            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = _tenantId, User = owner };
            var eaEvent = new EffectiveAuthorizationGrantedEvent() { From = new DateTime(2018, 1, 1), EffectiveAuthorization = effectiveAuthorization };

            await eaEventsStorate.WriteRawEventAsync(eaEvent); //event needs to be previously writteng to storage.

            var dataEnrichmentService = new DataEnrichmentService(eatimelineFactory, prService, rpStorage);

            //Act & Assert
            await Assert.ThrowsExceptionAsync<NullReferenceException>(() => dataEnrichmentService.AddEffectiveAuthorizationAsync(eaEvent));
        }

        [TestMethod]
        public async Task AddEffectiveAuthorizationAsync_Event_Stored_With_Invalid_Authorization_No_Permission_Throws_NullReferenceException()
        {
            //Arrange
            var eaEventsStorate = new RawEventInMemoryStorage();

            var eaHandlerFactory = new EffectiveAuthorizationHandlerFactory();
            eaHandlerFactory.RegisterHandler(typeof(EffectiveAuthorizationGrantedEvent), new PermissionGrantedHandler());

            var eatimelineFactory = new EffectiveAuthorizationTimelineFactory(eaEventsStorate, eaHandlerFactory);

            var prService = new PersonalInfoEnrichmentService(new MockPersonalLocalStorage(), new MockPersonalInfoExternalServiceFactory(), new MockInMemoryLogger());
            var rpStorage = new ReportingInMemoryStorage();

            var permission = new Permission() { Application = _defaultApplication, Id = _defaultPermissionId, Description = _defaultDescription };
            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = _tenantId, Permission = permission };
            var eaEvent = new EffectiveAuthorizationGrantedEvent() { From = new DateTime(2018, 1, 1), EffectiveAuthorization = effectiveAuthorization };

            await eaEventsStorate.WriteRawEventAsync(eaEvent); //event needs to be previously writteng to storage.

            var dataEnrichmentService = new DataEnrichmentService(eatimelineFactory, prService, rpStorage);

            //Act & Assert
            await Assert.ThrowsExceptionAsync<NullReferenceException>(() => dataEnrichmentService.AddEffectiveAuthorizationAsync(eaEvent));
        }
    }
}
