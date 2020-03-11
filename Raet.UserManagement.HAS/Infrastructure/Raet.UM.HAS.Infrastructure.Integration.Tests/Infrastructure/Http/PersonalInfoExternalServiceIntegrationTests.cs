using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raet.UM.HAS.Infrastructure.Http;
using Raet.UM.HAS.Infrastructure.Http.Common;

namespace Raet.UM.HAS.Infrastructure.Integration.Tests.Infrastructure.Http
{
    [TestClass]
    public class PersonalInfoExternalServiceIntegrationTests
    {
        //[TestMethod]
        public async Task ExecuteServiceOnPortalAPI_Authenticated_OwnerFound_ReturnsPersonalInfo()
        {
            //  Warning: This test is tied to existing information being retrieved from the External Resolver Api
            var authSettings = new AuthenticationSettings(InfrastructureConfiguration.AuthProviderUri, 
                                                          InfrastructureConfiguration.AuthProviderClient, 
                                                          InfrastructureConfiguration.AuthProviderSecret);

            IAuthenticationProvider authProvider = new AuthenticationProvider(authSettings);

            var service = new PersonalInfoExternalService(InfrastructureConfiguration.PortalExternalResolveUri, authProvider, new Mock<ILogger>().Object);
            var personalInfo = await service.FindPersonalInfoAsync("RP135685");

            Assert.IsNotNull(personalInfo);
            Assert.AreEqual("J", personalInfo.Initials);
            Assert.AreEqual("Vries", personalInfo.LastNameAtBirth);
            Assert.AreEqual("V", personalInfo.LastNameAtBirthPrefix);
            Assert.AreEqual(1969, personalInfo.BirthDate.Year);
            Assert.AreEqual(8, personalInfo.BirthDate.Month);
            Assert.AreEqual(5, personalInfo.BirthDate.Day);
        }

        [TestMethod]
        public async Task ExecuteServiceOnPortalAPI_NotAuthenticated_ReturnsNull()
        {
            var authSettings = new AuthenticationSettings(InfrastructureConfiguration.AuthProviderUri,
                                                          InfrastructureConfiguration.AuthProviderClient, 
                                                          "InvalidSecret");

            IAuthenticationProvider authProvider = new AuthenticationProvider(authSettings);

            var service = new PersonalInfoExternalService(InfrastructureConfiguration.PortalExternalResolveUri, authProvider, new Mock<ILogger>().Object);
            var personalInfo = await service.FindPersonalInfoAsync("RP135685");

            Assert.IsNull(personalInfo);
        }

        [TestMethod]
        public async Task ExecuteServiceOnPortalAPI_Authenticated_UserNotFound_ReturnsNull()
        {
            var authSettings = new AuthenticationSettings(InfrastructureConfiguration.AuthProviderUri,
                                                          InfrastructureConfiguration.AuthProviderClient,
                                                          InfrastructureConfiguration.AuthProviderSecret);

            IAuthenticationProvider authProvider = new AuthenticationProvider(authSettings);

            var service = new PersonalInfoExternalService(InfrastructureConfiguration.PortalExternalResolveUri, authProvider, new Mock<ILogger>().Object);
            var personalInfo = await service.FindPersonalInfoAsync("InvalidUser");

            Assert.IsNull(personalInfo);
        }
    }
}
