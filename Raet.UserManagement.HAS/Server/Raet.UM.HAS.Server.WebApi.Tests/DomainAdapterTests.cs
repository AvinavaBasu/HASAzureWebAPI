using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raet.UM.HAS;
using Raet.UM.HAS.DTOs;
using Raet.UM.HAS.Server.WebApi.Adapters;
using Moq;


namespace Raet.UM.HAS.Server.WebApi.Tests
{
    [TestClass]
    public class DomainAdapterTests
    {
        private readonly string string1 = "String1";
        private readonly string string2 = "String2";
        private readonly string string3 = "String3";
        private readonly string string4 = "String4";
        private readonly string string5 = "String5";
        private readonly string string6 = "String6";
        private readonly string string7 = "String7";
        private readonly string string8 = "String8";

        private readonly DateTime referenceDateTime = new DateTime(2018, 1, 1);

        [TestMethod]
        public void MapExternalId_Returns_Domain_ExternalId_With_Valid_Values()
        {
            //Arrange
            var dtoExternalId = new ExternalId(string1, string2);

            //Act
            var domainExternalId = DomainAdapter.MapExternalId(dtoExternalId);

            //Assert
            Assert.IsNotNull(domainExternalId);
            Assert.AreEqual(dtoExternalId.Id, domainExternalId.Id);
            Assert.AreEqual(dtoExternalId.Context, domainExternalId.Context);
        }

        [TestMethod]
        public void MapPermission_Returns_Domain_Permission_With_Valid_Values()
        {
            //Arrange
            var dtoPermission = new Permission(string1, string2, string3);

            //Act
            var domainPermission = DomainAdapter.MapPermission(dtoPermission);

            //Assert
            Assert.IsNotNull(domainPermission);
            Assert.AreEqual(dtoPermission.Application, domainPermission.Application);
            Assert.AreEqual(dtoPermission.Description, domainPermission.Description);
            Assert.AreEqual(dtoPermission.Id, domainPermission.Id);
        }

        [TestMethod]
        public void MapEffectiveAuthorization_DTO_Without_Target_Returns_Domain_EffectiveAuthorization_With_Valid_Values()
        {
            //Arrange
            var dtoEffectiveAuthorization = new EffectiveAuthorization(string1, new ExternalId(string2, string3), new Permission(string4, string5, string6));

            //Act
            var domainEffectiveAuthorization = DomainAdapter.MapEffectiveAuthorization(dtoEffectiveAuthorization);

            //Assert
            Assert.IsNotNull(domainEffectiveAuthorization);
            Assert.IsNotNull(domainEffectiveAuthorization.Permission);
            Assert.IsNotNull(domainEffectiveAuthorization.User);

            Assert.IsNull(domainEffectiveAuthorization.Target);

            Assert.AreEqual(string1, domainEffectiveAuthorization.TenantId);

            Assert.AreEqual(dtoEffectiveAuthorization.Permission.Application, domainEffectiveAuthorization.Permission.Application);
            Assert.AreEqual(dtoEffectiveAuthorization.Permission.Description, domainEffectiveAuthorization.Permission.Description);
            Assert.AreEqual(dtoEffectiveAuthorization.Permission.Id, domainEffectiveAuthorization.Permission.Id);

            Assert.AreEqual(dtoEffectiveAuthorization.User.Context, domainEffectiveAuthorization.User.Context);
            Assert.AreEqual(dtoEffectiveAuthorization.User.Id, domainEffectiveAuthorization.User.Id);

        }

        [TestMethod]
        public void MapEffectiveAuthorization_DTO_With_Target_Returns_Domain_EffectiveAuthorization_With_Valid_Values()
        {
            //Arrange
            var dtoEffectiveAuthorization = new EffectiveAuthorization(string1, new ExternalId(string2, string3), new Permission(string4, string5, string6), new ExternalId(string7, string8));

            //Act
            var domainEffectiveAuthorization = DomainAdapter.MapEffectiveAuthorization(dtoEffectiveAuthorization);

            //Assert
            Assert.IsNotNull(domainEffectiveAuthorization);
            Assert.IsNotNull(domainEffectiveAuthorization.Permission);
            Assert.IsNotNull(domainEffectiveAuthorization.User);
            Assert.IsNotNull(domainEffectiveAuthorization.Target);

            Assert.AreEqual(string1, domainEffectiveAuthorization.TenantId);

            Assert.AreEqual(dtoEffectiveAuthorization.Permission.Application, domainEffectiveAuthorization.Permission.Application);
            Assert.AreEqual(dtoEffectiveAuthorization.Permission.Description, domainEffectiveAuthorization.Permission.Description);
            Assert.AreEqual(dtoEffectiveAuthorization.Permission.Id, domainEffectiveAuthorization.Permission.Id);

            Assert.AreEqual(dtoEffectiveAuthorization.Target.Context, domainEffectiveAuthorization.Target.Context);
            Assert.AreEqual(dtoEffectiveAuthorization.Target.Id, domainEffectiveAuthorization.Target.Id);

            Assert.AreEqual(dtoEffectiveAuthorization.User.Context, domainEffectiveAuthorization.User.Context);
            Assert.AreEqual(dtoEffectiveAuthorization.User.Id, domainEffectiveAuthorization.User.Id);
        }

        [TestMethod]
        public void MapEvent_DTO_EffectiveAuthorizationGranted_Returns_Domain_EffectiveAuthorizationGranted_With_Valid_Values()
        {
            //Arrange
            var dtoEffectiveAuthorizationGrantedEvent = new EffectiveAuthorizationGrantedEvent();
            dtoEffectiveAuthorizationGrantedEvent.EffectiveAuthorization = new EffectiveAuthorization(string1, new ExternalId(string2, string3), new Permission(string4, string5, string6), new ExternalId(string7, string8));
            dtoEffectiveAuthorizationGrantedEvent.FromDateTime = referenceDateTime;

            //Act
            var domainEffectiveAuthorizationGrantedEvent = DomainAdapter.MapEvent(dtoEffectiveAuthorizationGrantedEvent);

            //Assert
            Assert.IsNotNull(domainEffectiveAuthorizationGrantedEvent);
            Assert.IsNotNull(domainEffectiveAuthorizationGrantedEvent.EffectiveAuthorization);

            Assert.AreEqual(dtoEffectiveAuthorizationGrantedEvent.FromDateTime, domainEffectiveAuthorizationGrantedEvent.From);

            Assert.AreEqual(dtoEffectiveAuthorizationGrantedEvent.EffectiveAuthorization.TenantId, domainEffectiveAuthorizationGrantedEvent.EffectiveAuthorization.TenantId);

            Assert.AreEqual(dtoEffectiveAuthorizationGrantedEvent.EffectiveAuthorization.Permission.Application, domainEffectiveAuthorizationGrantedEvent.EffectiveAuthorization.Permission.Application);
            Assert.AreEqual(dtoEffectiveAuthorizationGrantedEvent.EffectiveAuthorization.Permission.Description, domainEffectiveAuthorizationGrantedEvent.EffectiveAuthorization.Permission.Description);
            Assert.AreEqual(dtoEffectiveAuthorizationGrantedEvent.EffectiveAuthorization.Permission.Id, domainEffectiveAuthorizationGrantedEvent.EffectiveAuthorization.Permission.Id);

            Assert.AreEqual(dtoEffectiveAuthorizationGrantedEvent.EffectiveAuthorization.Target.Context, domainEffectiveAuthorizationGrantedEvent.EffectiveAuthorization.Target.Context);
            Assert.AreEqual(dtoEffectiveAuthorizationGrantedEvent.EffectiveAuthorization.Target.Id, domainEffectiveAuthorizationGrantedEvent.EffectiveAuthorization.Target.Id);

            Assert.AreEqual(dtoEffectiveAuthorizationGrantedEvent.EffectiveAuthorization.User.Context, domainEffectiveAuthorizationGrantedEvent.EffectiveAuthorization.User.Context);
            Assert.AreEqual(dtoEffectiveAuthorizationGrantedEvent.EffectiveAuthorization.User.Id, domainEffectiveAuthorizationGrantedEvent.EffectiveAuthorization.User.Id);
        }

        [TestMethod]
        public void MapEvent_DTO_EffectiveAuthorizationRevoked_Returns_Domain_EffectiveAuthorizationRevoked_With_Valid_Values()
        {
            //Arrange
            var dtoEffectiveAuthorizationRevokedEvent = new EffectiveAuthorizationRevokedEvent();
            dtoEffectiveAuthorizationRevokedEvent.EffectiveAuthorization = new EffectiveAuthorization(string1, new ExternalId(string2, string3), new Permission(string4, string5, string6), new ExternalId(string7, string8));
            dtoEffectiveAuthorizationRevokedEvent.UntilDateTime = referenceDateTime;

            //Act
            var domainEffectiveAuthorizationRevokedEvent = DomainAdapter.MapEvent(dtoEffectiveAuthorizationRevokedEvent);

            //Assert
            Assert.IsNotNull(domainEffectiveAuthorizationRevokedEvent);
            Assert.IsNotNull(domainEffectiveAuthorizationRevokedEvent.EffectiveAuthorization);

            Assert.AreEqual(dtoEffectiveAuthorizationRevokedEvent.UntilDateTime, domainEffectiveAuthorizationRevokedEvent.Until);

            Assert.AreEqual(dtoEffectiveAuthorizationRevokedEvent.EffectiveAuthorization.TenantId, domainEffectiveAuthorizationRevokedEvent.EffectiveAuthorization.TenantId);

            Assert.AreEqual(dtoEffectiveAuthorizationRevokedEvent.EffectiveAuthorization.Permission.Application, domainEffectiveAuthorizationRevokedEvent.EffectiveAuthorization.Permission.Application);
            Assert.AreEqual(dtoEffectiveAuthorizationRevokedEvent.EffectiveAuthorization.Permission.Description, domainEffectiveAuthorizationRevokedEvent.EffectiveAuthorization.Permission.Description);
            Assert.AreEqual(dtoEffectiveAuthorizationRevokedEvent.EffectiveAuthorization.Permission.Id, domainEffectiveAuthorizationRevokedEvent.EffectiveAuthorization.Permission.Id);

            Assert.AreEqual(dtoEffectiveAuthorizationRevokedEvent.EffectiveAuthorization.Target.Context, domainEffectiveAuthorizationRevokedEvent.EffectiveAuthorization.Target.Context);
            Assert.AreEqual(dtoEffectiveAuthorizationRevokedEvent.EffectiveAuthorization.Target.Id, domainEffectiveAuthorizationRevokedEvent.EffectiveAuthorization.Target.Id);

            Assert.AreEqual(dtoEffectiveAuthorizationRevokedEvent.EffectiveAuthorization.User.Context, domainEffectiveAuthorizationRevokedEvent.EffectiveAuthorization.User.Context);
            Assert.AreEqual(dtoEffectiveAuthorizationRevokedEvent.EffectiveAuthorization.User.Id, domainEffectiveAuthorizationRevokedEvent.EffectiveAuthorization.User.Id);
        }
    }
}
