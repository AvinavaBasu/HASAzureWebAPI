using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raet.UM.HAS.Infrastructure.Http;
using Raet.UM.HAS.Infrastructure.Http.Common;
using Raet.UM.HAS.Infrastructure.Storage.Table;

namespace Raet.UM.HAS.Infrastructure.Tests.Http
{
    [TestClass]
    public class PersonalInfoExternalServiceFactoryTests
    {
        [TestMethod]
        public void PersonalInfoExternalServiceFactory_ResolverAPIFoundForProvidedContext_ReturnsAPersonalInfoExternalService()
        {
            var exampleUri = "http://localhost/api/persons/{id}/personaldetails";

            Mock<IContextMappingLocalStorage> contextMappingMock = new Mock<IContextMappingLocalStorage>();
            contextMappingMock.Setup(x => x.Resolve("Youforce.Users")).Returns(exampleUri);

            Mock<IAuthenticationProvider> authProviderMock = new Mock<IAuthenticationProvider>();

            var factory = new PersonalInfoExternalServiceFactory(contextMappingMock.Object, authProviderMock.Object, new Mock<ILogger>().Object);
            var service = factory.Resolve("Youforce.Users");

            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void PersonalInfoExternalServiceFactory_ResolverAPINotFoundForProvidedContext_ReturnsANull()
        {
            var exampleUri = "http://localhost/api/persons/{id}/personaldetails";

            Mock<IContextMappingLocalStorage> contextMappingMock = new Mock<IContextMappingLocalStorage>();
            contextMappingMock.Setup(x => x.Resolve("Youforce.Users")).Returns(exampleUri);

            Mock<IAuthenticationProvider> authProviderMock = new Mock<IAuthenticationProvider>();

            var factory = new PersonalInfoExternalServiceFactory(contextMappingMock.Object, authProviderMock.Object, new Mock<ILogger>().Object);
            var service = factory.Resolve("OtherContext.Users");

            Assert.IsNull(service);
        }
    }
}
