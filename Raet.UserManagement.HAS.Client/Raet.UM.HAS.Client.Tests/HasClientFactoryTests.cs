using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raet.UM.HAS.DTOs;
using Raet.UM.HAS.Client.Http;

namespace Raet.UM.HAS.Client.Tests
{
    [TestClass]
    public class HasClientFactoryTests
    {
        private readonly string _defaultClientId = "123456789xyz";
        private readonly string _defaultClientSecret = "Q8ojYHoVydRmRDR5ZdKeJf9XawlvbFX";

        [TestMethod]
        public void CreateApiClient_RightFormat_URL_Returns_IPushClient()
        {
            var client = HasClientFactory.CreateApiClient(new HttpClientSettings(new Uri("https://requestb.in/1ipzzzf1"), _defaultClientId, _defaultClientSecret, new Uri("https://www.some-unexisting-url.com/as/token.oauth2"), "x-client-id-example"));

            Assert.IsInstanceOfType(client, typeof(IPushClient));
        }

        [TestMethod]
        public void CreateApiClient_WrongFormat_Url_throws_UriFormatException()
        {
            Assert.ThrowsException<UriFormatException>(() => HasClientFactory.CreateApiClient(new HttpClientSettings(new Uri("123wrong/api"), _defaultClientId, _defaultClientSecret, new Uri("https://www.some-unexisting-url.com/as/token.oauth2"), "x-client-id-example")));
        }
    }
}
