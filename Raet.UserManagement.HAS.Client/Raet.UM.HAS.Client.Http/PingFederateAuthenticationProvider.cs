using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Raet.UM.HAS.Client.Http
{
    public class PingFederateAuthenticationProvider : IAuthenticationProvider
    {
        private const string _queryString = "grant_type=client_credentials";

        public string RequestUrl { get; private set; }

        public string ClientId { get; private set; }

        public string ClientSecret { get; private set; }

        public string XClientIdHeader { get; private set; }

        private JwtSecurityToken _jwt;

        public PingFederateAuthenticationProvider(string requestUrl, string clientId, string clientSecret, string xClientIdHeader)
        {
            RequestUrl = requestUrl;
            ClientId = clientId;
            ClientSecret = clientSecret;
            XClientIdHeader = xClientIdHeader;

            _jwt = null;
        }

        public string GetJwt()
        {
            if (_jwt == null || _jwt.ValidTo <= DateTime.Now.ToUniversalTime())
            {
                var encodedCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($@"{ClientId}:{ClientSecret}"));
                HttpResponseMessage response;
                string result;

                using (var handler = new WebRequestHandler())
                {
                    //this is to avoid the "certificate doesn't match with procedure" error.
                    //TODO this line of code should be removed by a more adequate solution, as proposed by System Architect
                    handler.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                    using (var client = new HttpClient(handler))
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encodedCredentials);
                        client.DefaultRequestHeaders.Add("x-client-id", XClientIdHeader);

                        response = client.PostAsync($"{RequestUrl}?{_queryString}", null).Result;
                    }
                }

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _jwt = null;
                    return string.Empty;
                }

                result = response.Content.ReadAsStringAsync().Result;

                var resultDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
                _jwt = new JwtSecurityToken(resultDictionary["access_token"].ToString());
            }

            return _jwt.RawData;
        }

        public async Task<string> GetJwtAsync()
        {
            return await Task.Run(() =>
            {
                return GetJwt();
            });
        }
    }
}
