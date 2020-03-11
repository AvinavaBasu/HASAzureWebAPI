using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raet.UM.HAS.DTOs;

namespace Raet.UM.HAS.Client.Tests.DTOs
{
    [TestClass]
    public class EffectiveAuthorizationTests
    {
        private readonly string _defaultTenant = "Company01";
        private readonly string _defaultContext = "CONTEXTS.Default";
        private readonly string _companyContext = "YOUFORCE.Companies";
        private readonly string _defaultUserId1 = "User001";
        private readonly string _defaultPersonId1 = "Person001";
        private readonly string _defaultIdentifier = "Identifier01";
        private readonly string _defaultApplication = "Application01";

        [TestMethod]
        public void Contructor_Returns_EffectiveAuthorization_With_Values_And_Properties()
        {
            //Arrange
            var user = new ExternalId(_defaultContext, _defaultUserId1);
            var permission = new Permission(_defaultIdentifier, _defaultApplication);
            var target = new ExternalId(_defaultContext, _defaultPersonId1);

            //Act
            var authorization = new EffectiveAuthorization(_defaultTenant, user, permission, target);

            //Assert
            Assert.IsInstanceOfType(authorization, typeof(EffectiveAuthorization));
            Assert.AreEqual(_defaultTenant, authorization.TenantId);
            Assert.AreEqual(user, authorization.User);
            Assert.AreEqual(permission, authorization.Permission);
            Assert.AreEqual(target, authorization.Target);
        }

        [TestMethod]
        public void Contstructor_With_No_Target_Returns_EffectiveAuthorization_Without_Target()
        {
            //Arrange
            var user = new ExternalId(_defaultContext, _defaultUserId1);
            var permission = new Permission(_defaultIdentifier, _defaultApplication);

            //Act
            var authorization = new EffectiveAuthorization(_defaultTenant, user, permission);

            //Assert
            Assert.IsInstanceOfType(authorization, typeof(EffectiveAuthorization));
            Assert.AreEqual(_defaultTenant, authorization.TenantId);
            Assert.AreEqual(user, authorization.User);
            Assert.AreEqual(permission, authorization.Permission);
            Assert.AreEqual(null, authorization.Target);
        }

        [TestMethod]
        public void ExtensionMethod_AddTargetCompany_Adds_Target_With_Right_Company_Data()
        {
            //Arrange
            var user = new ExternalId(_defaultContext, _defaultUserId1);
            var permission = new Permission(_defaultIdentifier, _defaultApplication);
            var authorization = new EffectiveAuthorization(_defaultTenant, user, permission);

            //Assert
            authorization.AddTargetCompany(_defaultTenant);

            Assert.IsNotNull(authorization.Target);
            Assert.AreEqual(_companyContext, authorization.Target.Context);
            Assert.AreEqual(_defaultTenant, authorization.Target.Id);
        }
    }
}
