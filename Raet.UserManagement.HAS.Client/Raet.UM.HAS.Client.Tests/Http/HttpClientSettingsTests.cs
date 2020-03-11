using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raet.UM.HAS.DTOs;
using Raet.UM.HAS.Client.Http;

namespace Raet.UM.HAS.Client.Tests.Http
{
    [TestClass]
    public class HttpClientSettingsTests
    {
        private readonly Uri _urlBase = new Uri("https://www.somebaseuri.com/somedomain/");
        private readonly string _defaultClientId = "an-imaginary-clientId";
        private readonly string _defaultClientSecret = "an-imaginary-clientSecret";
        private readonly Uri _authority = new Uri("http://www.someauthority-pingfederate/");
        private readonly string _xClientIdHeader = "x-client-id-example";

        [TestMethod]
        public void Contructor_Right_Parameters_Returns_HttpClientSettings_With_Right_Properties()
        {
            //Arrange
            //all necessary variables already set

            //Act
            var httpSettings = new HttpClientSettings(_urlBase, _defaultClientId, _defaultClientSecret, _authority, _xClientIdHeader);

            //Assert
            Assert.IsInstanceOfType(httpSettings, typeof(HttpClientSettings));
            Assert.AreEqual(_urlBase, httpSettings.BaseUrl);
            Assert.AreEqual(_defaultClientId, httpSettings.ClientId);
            Assert.AreEqual(_defaultClientSecret, httpSettings.ClientSecret);
            Assert.AreEqual(_authority, httpSettings.Authority);
            Assert.AreEqual(_xClientIdHeader, httpSettings.XClientIdHeader);
        }

        [TestMethod]
        public void Constructor_Null_Parameters_Returns_HttpClientSettings_Returns_HttpClientSettings_With_Null_Properties()
        {
            //Act
            var httpSettings = new HttpClientSettings(null, null, null, null, null);

            //Assert
            Assert.IsInstanceOfType(httpSettings, typeof(HttpClientSettings));
            Assert.AreEqual(null, httpSettings.BaseUrl);
            Assert.AreEqual(null, httpSettings.ClientId);
            Assert.AreEqual(null, httpSettings.ClientSecret);
            Assert.AreEqual(null, httpSettings.Authority);
            Assert.AreEqual(null, httpSettings.XClientIdHeader);
        }
    }
}
