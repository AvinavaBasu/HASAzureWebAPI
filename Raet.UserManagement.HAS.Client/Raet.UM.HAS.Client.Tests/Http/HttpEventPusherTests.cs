using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raet.UM.HAS.DTOs;
using Raet.UM.HAS.Client.Http;
using Moq;

namespace Raet.UM.HAS.Client.Tests.Http
{
    [TestClass]
    public class HttpEventPusherTests
    {
        private readonly HttpClientSettings _httpClientSettings = new HttpClientSettings(new Uri("https://www.someuri.com/"), "some-clientId", "some-clientSecret", new Uri("https://www.someauthority.com/"), "x-client-id-example");

        [TestMethod]
        public void Contructor_Right_Parameters_Returns_HttpEventPusher()
        {
            //Arrange
            Mock<IAuthenticationProvider> _authenticationProviderMock = new Mock<IAuthenticationProvider>();

            //Act
            var httpEventPusher = new HttpEventPusher<EffectiveAuthorizationEvent>(_httpClientSettings, _authenticationProviderMock.Object);

            //Assert
            Assert.IsInstanceOfType(httpEventPusher, typeof(HttpEventPusher<EffectiveAuthorizationEvent>));
        }

        [TestMethod]
        public void Contructor_Null_Parameters_Returns_NullReferenceException()
        {
            //Assert
            Assert.ThrowsException<NullReferenceException>(() => new HttpEventPusher<EffectiveAuthorizationEvent>(null, null));
        }

        [TestMethod]
        public void Constructor_Null_Parameters_But_HttpSettings_Returns_HttpEventPusher()
        {
            //Act
            var httpEventPusher = new HttpEventPusher<EffectiveAuthorizationEvent>(_httpClientSettings, null);

            //Assert
            Assert.IsInstanceOfType(httpEventPusher, typeof(HttpEventPusher<EffectiveAuthorizationEvent>));
        }
    }
}
