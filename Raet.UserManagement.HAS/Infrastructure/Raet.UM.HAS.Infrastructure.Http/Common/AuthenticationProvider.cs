using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Infrastructure.Http.Common
{
    public class AuthenticationProvider : IAuthenticationProvider
    {
        private JwtSecurityToken jwt;

        private bool IsJwtInvalidOrExpired => jwt == null || jwt.ValidTo <= DateTime.Now.ToUniversalTime();

        private readonly AuthenticationSettings settings;

        public AuthenticationProvider(AuthenticationSettings settings)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            this.settings = settings;
            jwt = null;
        }

        public async Task<string> GetJwtAsync()
        {
            return await Task.Run(() =>
            {
                return GetJwt();
            });
        }

        public string GetJwt()
        {
            if (IsJwtInvalidOrExpired)
            {
                var response = RequestNewJwt();
                jwt = response.IsSuccessful ? CreateNewJwt(response.Content) : null;
            }

            return jwt != null ? jwt.RawData : string.Empty;
        }

        private IRestResponse RequestNewJwt()
        {
            var client = new RestClient(settings.RequestUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", $"grant_type=client_credentials&client_id={settings.ClientId}&client_secret={settings.ClientSecret}", ParameterType.RequestBody);

            return client.Execute(request);
        }

        private JwtSecurityToken CreateNewJwt(string responseContent)
        {
            var responseDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);
            return new JwtSecurityToken(responseDictionary["access_token"].ToString());
        }
    }
}