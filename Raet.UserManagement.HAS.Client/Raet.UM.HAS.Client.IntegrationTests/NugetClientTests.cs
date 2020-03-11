using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raet.UM.HAS;
using Raet.UM.HAS.Client;
using Raet.UM.HAS.Client.Http;


namespace Raet.UM.HAS.Client.IntegrationTests
{
    [TestClass]
    public class NugetClientTests
    {
        private readonly string _endppoint = "https://raetgdpr-dev.azurewebsites.net/api/EffectiveAuthorizationEvents/";
        private readonly string _authenticationUrl = TestInfrastructureConfiguration.AuthProviderUri;
        private readonly string _clientId = TestInfrastructureConfiguration.AuthProviderClient;
        private readonly string _clientSecret = TestInfrastructureConfiguration.AuthProviderSecret;
        private readonly string _xClientIdHeader = "x-client-id-example";
        private readonly string _tenantId = "NuGetClientTests";

        [TestMethod]
        public void HasClientNugetPackagePushAuthorizationsReturnsNoErrorMessageAndAndCompliesSLA()
        {
            #region arrange 
            int requestsQtty = 100;
            int expectedSLA = requestsQtty /2;
            int errorCount = 0;
            var watch = new Stopwatch();
            

            var theClient = HasClientFactory.CreateApiClient(GetClientSettings());
            var effectiveAuthorization = EffectiveAuthorizationFactory.Create(_tenantId);
            var timestamp = DateTime.Now;
            #endregion arrange 

            #region act
            watch.Start();
            for (int i = 0; i < requestsQtty; i++)
            {
                var grantResponseGranted = theClient.PushEffectiveAuthorizationGrantedAsync(effectiveAuthorization, timestamp).Result;

                if (!grantResponseGranted.IsSuccess) errorCount++;
            }
            watch.Stop();
            var seconds = watch.Elapsed.Seconds;
            #endregion act

            //assert
            Assert.AreEqual(0, errorCount, $"{errorCount} errors on {requestsQtty} requests");
            Assert.IsTrue(seconds < expectedSLA, "Average requests timing exceeds expected SLA");
        }

        [TestMethod]
        public void Client_PushEffectiveAuthorizationGranted_Valid_Event_Returns_CreatedEvent_Id()
        {
            //arrange
            var theClient = HasClientFactory.CreateApiClient(GetClientSettings());
            var effectiveAuthorization = EffectiveAuthorizationFactory.Create( _tenantId);

            //act
            var grantResponseGranted = theClient.PushEffectiveAuthorizationGranted(effectiveAuthorization, DateTime.Now);

            Assert.AreEqual(true, grantResponseGranted.IsSuccess);
            Assert.AreEqual(true, Guid.TryParse(grantResponseGranted.Message, out Guid id));
        }

        [TestMethod]
        public async Task Client_PushEffectiveAuthorizationGrantedAsync_Valid_Event_Returns_CreatedEvent_Id()
        {
            //arrange
            var theClient = HasClientFactory.CreateApiClient(GetClientSettings());
            var effectiveAuthorization = EffectiveAuthorizationFactory.Create(_tenantId);

            //act
            var grantResponseGranted = await theClient.PushEffectiveAuthorizationGrantedAsync(effectiveAuthorization, DateTime.Now);

            //assert
            Assert.AreEqual(true, grantResponseGranted.IsSuccess);
            Assert.AreEqual(true, Guid.TryParse(grantResponseGranted.Message, out Guid id));
        }

        [TestMethod]
        public void Client_PushEffectiveAuthorizationRevoked_Valid_Event_Returns_CreatedEvent_Id()
        {
            //arrange
            var theClient = HasClientFactory.CreateApiClient(GetClientSettings());
            var effectiveAuthorization = EffectiveAuthorizationFactory.Create( _tenantId);

            //act
            var grantResponseRevoked = theClient.PushEffectiveAuthorizationRevoked(effectiveAuthorization, DateTime.Now);

            //assert
            Assert.AreEqual(true, grantResponseRevoked.IsSuccess);
            Assert.AreEqual(true, Guid.TryParse(grantResponseRevoked.Message, out Guid id));
        }

        [TestMethod]
        public async Task Client_PushEffectiveAuthorizationRevokedAsync_Valid_Event_Returns_CreatedEvent_Id()
        {
            //arrange
            var theClient = HasClientFactory.CreateApiClient(GetClientSettings());
            var effectiveAuthorization = EffectiveAuthorizationFactory.Create(_tenantId);

            //act
            var grantResponseRevoked = await theClient.PushEffectiveAuthorizationRevokedAsync(effectiveAuthorization, DateTime.Now);

            //assert
            Assert.AreEqual(true, grantResponseRevoked.IsSuccess);
            Assert.AreEqual(true, Guid.TryParse(grantResponseRevoked.Message, out Guid id));
        }

        [TestMethod]
        public async Task Client_PushEffectiveAuthorizationGrantedAsynk_Unauthorized_Returns_ErrorMessage()
        {
            //arrange
            var settings = GetClientSettings(_endppoint, _clientId, "InvalidSecret", _authenticationUrl, _xClientIdHeader);
            
            var theClient = HasClientFactory.CreateApiClient(settings);
            var effectiveAuthorization = EffectiveAuthorizationFactory.Create(_tenantId);

            //act
            var eventResponse = await theClient.PushEffectiveAuthorizationGrantedAsync(effectiveAuthorization, DateTime.Now);

            Assert.AreEqual(false, eventResponse.IsSuccess);
            Assert.AreEqual("Authentication failed!. Please check provided Authority url and credentials", eventResponse.Message);
        }

        private HttpClientSettings GetClientSettings()
        {
            return GetClientSettings(_endppoint, _clientId, _clientSecret, _authenticationUrl, _xClientIdHeader);
        }

        private HttpClientSettings GetClientSettings(string endpoint, string clientId, string clientSecret, string authUrl, string xClientIdHeader)
        {
            return new HttpClientSettings(new Uri(endpoint),
                                         clientId,
                                         clientSecret,
                                         new Uri(authUrl),
                                         xClientIdHeader);
        }
    }
}

