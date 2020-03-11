using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raet.UM.HAS.Infrastructure.Http.Common;
using RestSharp;
using System.Diagnostics;


namespace Raet.UM.HAS.Server.WebApi.Tests
{
    [TestClass]
    public class EffectiveAuthorizationControllerIntegrationTests
    {
        private readonly string _endppoint = "https://raetgdpr-dev.azurewebsites.net/api/EffectiveAuthorizationEvents/";
        private readonly string _action = "granted";
        private readonly string _authenticationUrl = TestInfrastructureConfiguration.AuthProviderUri;
        private readonly string _clientId = TestInfrastructureConfiguration.AuthProviderClient;
        private readonly string _clientSecret = TestInfrastructureConfiguration.AuthProviderSecret;


        [TestMethod, Ignore]
        public void HttpClientPushAuthorizationsEventsReturnsNoErrorAndCompliesSLA()
        {
            #region arrange 
            int requestsQtty = 100;
            int expectedSLA = requestsQtty / 2;
            int errorCount = 0;
            var watch = new Stopwatch();

            var autorizationEvent = EventFactory.BuildEventByType("granted");
            var client = new RestClient(new Uri($"{_endppoint}{_action}"));
            var request = Request;
            request.AddBody(autorizationEvent);
            #endregion arrange 

            #region act
            watch.Start();
            for (int i = 0; i < requestsQtty; i++)
            {
                var requestResult = client.Execute(request);
                if (!requestResult.IsSuccessful)
                {
                    errorCount++;
                }
            }
            watch.Stop();
            var seconds = watch.Elapsed.Seconds;
            #endregion act

            Assert.AreEqual(0, errorCount, $"{errorCount} errors on {requestsQtty} requests");
            Assert.IsTrue(seconds < expectedSLA, "Average requests timing exceeds expected SLA");

        }


        private AuthenticationSettings _settings;

        public AuthenticationSettings AuthSettings
        {
            get
            {

                if (_settings == null)
                {
                    _settings = new AuthenticationSettings(_authenticationUrl, _clientId, _clientSecret);
                }

                return _settings;
            }
        }


        private IAuthenticationProvider _provider;

        public IAuthenticationProvider Provider
        {
            get
            {
                if (_provider == null) { _provider = new AuthenticationProvider(AuthSettings); }
                return _provider;
            }
        }

        public RestRequest Request
        {
            get
            {
                var request = new RestRequest(Method.POST)
                {
                    RequestFormat = DataFormat.Json
                };
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", $"Bearer {Provider.GetJwt()}");
                return request;
            }
        }

        


    }
}
