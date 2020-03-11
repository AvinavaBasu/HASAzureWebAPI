using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raet.UM.HAS.DTOs;
using Raet.UM.HAS.Client.Http;
using Moq;


namespace Raet.UM.HAS.Client.Tests.Http
{
    [TestClass]
    public class PingFederateAuthenticationProviderTests
    {
        private readonly string _requestUrl = "https://www.somerequesturl.com/";
        private readonly string _clientId = "some-client-id";
        private readonly string _clientSecret = "some-client-secret";
        private readonly string _xClientIdHeader = "x-client-id-header";

        [TestMethod]
        public void Contructor_Right_Parameters_Returns_PingFederateAuthenticationProvider_Right_Properties()
        {
            //Arrange
            //all necessary variables already set

            //Act
            var pingFederateAuthenticationProvider = new PingFederateAuthenticationProvider(_requestUrl, _clientId, _clientSecret, _xClientIdHeader);

            //Assert
            Assert.IsInstanceOfType(pingFederateAuthenticationProvider, typeof(PingFederateAuthenticationProvider));
            Assert.AreEqual(_requestUrl, pingFederateAuthenticationProvider.RequestUrl);
            Assert.AreEqual(_clientId, pingFederateAuthenticationProvider.ClientId);
            Assert.AreEqual(_clientSecret, pingFederateAuthenticationProvider.ClientSecret);
            Assert.AreEqual(_xClientIdHeader, pingFederateAuthenticationProvider.XClientIdHeader);
        }

        [TestMethod]
        public void Contructor_Null_Parameters_Returns_PingFederateAuthenticationProvider_Null_Properties()
        {
            //Act
            var pingFederateAuthenticationProvider = new PingFederateAuthenticationProvider(null, null, null, null);

            //Assert
            Assert.IsInstanceOfType(pingFederateAuthenticationProvider, typeof(PingFederateAuthenticationProvider));
            Assert.AreEqual(null, pingFederateAuthenticationProvider.RequestUrl);
            Assert.AreEqual(null, pingFederateAuthenticationProvider.ClientId);
            Assert.AreEqual(null, pingFederateAuthenticationProvider.ClientSecret);
            Assert.AreEqual(null, pingFederateAuthenticationProvider.XClientIdHeader);
        }
    }
}
