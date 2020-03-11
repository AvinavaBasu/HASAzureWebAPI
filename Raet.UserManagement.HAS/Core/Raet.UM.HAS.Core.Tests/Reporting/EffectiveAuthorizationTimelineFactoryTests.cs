using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raet.UM.HAS.Core.Reporting;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Mocks;

namespace Raet.UM.HAS.Core.Tests.Reporting
{
    [TestClass]
    public class EffectiveAuthorizationTimelineFactoryTests
    {
        private readonly string _tenantId = "1234567";

        private readonly string _defaultId = "IC000001";
        private readonly string _defaultContext = "Youforce";

        private readonly string _defaultApplication = "YOUFORCE";
        private readonly string _defaultPermissionId = "HomeAccess";
        private readonly string _defaultDescription = "Home Acess";


        [TestMethod]
        public void Create_EffectiveAuthorization_With_EffectiveAuthorizationEvent_In_Storage_Returns_Timeline_With_EffectiveAuthorization()
        {
            //Arrange
            var eaEventsStorate = new RawEventInMemoryStorage();
            var eaHandlerFactory = new EffectiveAuthorizationHandlerFactory();
            eaHandlerFactory.RegisterHandler(typeof(EffectiveAuthorizationGrantedEvent), new PermissionGrantedHandler());

            var owner = new ExternalId() { Id = _defaultId, Context = _defaultContext };
            var permission = new Permission() { Application = _defaultApplication, Id = _defaultPermissionId, Description = _defaultDescription };
            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = _tenantId, Permission = permission, User = owner };
            var eaEvent = new EffectiveAuthorizationGrantedEvent() { From = new DateTime(2018, 1, 1), EffectiveAuthorization = effectiveAuthorization };

            eaEventsStorate.WriteRawEventAsync(eaEvent);

            var eaTimelineFactory = new EffectiveAuthorizationTimelineFactory(eaEventsStorate, eaHandlerFactory);

            //Act
            var eaTimeline = eaTimelineFactory.Create(effectiveAuthorization).Result;

            //Assert
            Assert.IsNotNull(eaTimeline);
            Assert.AreEqual(effectiveAuthorization, eaTimeline.EffectiveAuthorization);
        }

        [TestMethod]
        public void Create_EffectiveAuthorization_Without_EffectiveAuthorizationEvent_InStorage_Returns_Timeline_With_EffectiveAuthorization()
        {
            //Arrange
            var eaEventsStorate = new RawEventInMemoryStorage();
            var eaHandlerFactory = new EffectiveAuthorizationHandlerFactory();
            eaHandlerFactory.RegisterHandler(typeof(EffectiveAuthorizationGrantedEvent), new PermissionGrantedHandler());

            var owner = new ExternalId() { Id = _defaultId, Context = _defaultContext };
            var permission = new Permission() { Application = _defaultApplication, Id = _defaultPermissionId, Description = _defaultDescription };
            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = _tenantId, Permission = permission, User = owner };

            var eaTimelineFactory = new EffectiveAuthorizationTimelineFactory(eaEventsStorate, eaHandlerFactory);

            //Act
            var eaTimeline = eaTimelineFactory.Create(effectiveAuthorization).Result;

            //Assert
            Assert.IsNotNull(eaTimeline);
            Assert.AreEqual(effectiveAuthorization, eaTimeline.EffectiveAuthorization);
        }
    }
}
