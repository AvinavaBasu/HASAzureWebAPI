using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Raet.UM.HAS.Client.Interfaces;
using Raet.UM.HAS.DTOs;
using Newtonsoft.Json.Linq;

namespace Raet.UM.HAS.Client.Http
{
    public class HttpEventPusher<TEvent> : IEventPusher<TEvent> where TEvent : class
    {
        protected HttpClient _httpClient { get; private set; }

        private const string _grantUrl = "granted";

        private const string _revokeUrl = "revoked";

        private const string _unexpectedError = "An unexpected error occurred!. Please validate that provided urls are accessible from your environment";

        private readonly HttpClientSettings _settings;

        private readonly IAuthenticationProvider _provider;

        public HttpEventPusher(HttpClientSettings settings, IAuthenticationProvider provider)
        {
            _settings = settings;
            _provider = provider;

            _httpClient = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
            _httpClient.BaseAddress = _settings.BaseUrl;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("x-client-id", _settings.XClientIdHeader);
        }

        private async Task<HttpResponseMessage> PushToApiAsync(TEvent eventToPush, string pushMethodUrl)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _provider.GetJwtAsync());

            var response = await _httpClient.PostAsJsonAsync(pushMethodUrl, eventToPush);

            return response;
        }

        private HttpResponseMessage PushToApi(TEvent eventToPush, string pushMethodUrl)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _provider.GetJwt());

            var response = _httpClient.PostAsJsonAsync(pushMethodUrl, eventToPush).Result;

            return response;
        }

        private HttpEventPushResponse GetResponse(HttpResponseMessage message)
        {
            if (message.StatusCode == HttpStatusCode.NotFound)
                return new HttpEventPushResponse(false, "Resource Not found!. Please check provided HAS API base url");

            if (message.StatusCode == HttpStatusCode.Unauthorized ||
                message.StatusCode == HttpStatusCode.Forbidden)
                return new HttpEventPushResponse(false, "Authentication failed!. Please check provided authority url and credentials");

            bool isSuccess = message.StatusCode == HttpStatusCode.Created;
            var jObject = JObject.Parse(message.Content.ReadAsStringAsync().Result);

            string responseMessage = isSuccess ?
                jObject.SelectToken("id").ToString() :
                jObject.SelectToken("error").ToString();

            return new HttpEventPushResponse(isSuccess, responseMessage);
        }

        public IEventPushResponse PushGranted(TEvent pushedEvent)
        {
            try
            {
                var response = PushToApi(pushedEvent, _grantUrl);
                return GetResponse(response);
            }
            catch (Exception)
            {
                return new HttpEventPushResponse(false, _unexpectedError);
            }
        }

        public IEventPushResponse PushRevoked(TEvent pushedEvent)
        {
            try
            {
                var response = PushToApi(pushedEvent, _revokeUrl);
                return GetResponse(response);
            }
            catch (Exception)
            {
                return new HttpEventPushResponse(false, _unexpectedError);
            }
        }

        public async Task<IEventPushResponse> PushGrantedAsync(TEvent pushedEvent)
        {
            try
            {
                var response = await PushToApiAsync(pushedEvent, _grantUrl);
                return GetResponse(response);
            }
            catch (Exception)
            {
                return new HttpEventPushResponse(false, _unexpectedError);
            }
        }

        public async Task<IEventPushResponse> PushRevokedAsync(TEvent pushedEvent)
        {
            try
            {
                var response = await PushToApiAsync(pushedEvent, _revokeUrl);
                return GetResponse(response);
            }
            catch (Exception)
            {
                return new HttpEventPushResponse(false, _unexpectedError);
            }
        }
    }
}
