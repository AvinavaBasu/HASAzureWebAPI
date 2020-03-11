using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting;
using Raet.UM.HAS.Infrastructure.Http.Common;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Raet.UM.HAS.Core.Reporting.Interface;

namespace Raet.UM.HAS.Infrastructure.Http
{
    public class PersonalInfoExternalService : IPersonalInfoExternalService
    {
        private readonly string baseUri;

        private readonly IAuthenticationProvider provider;

        private readonly ILogger logger;

        public PersonalInfoExternalService(string baseUri, IAuthenticationProvider provider, ILogger logger)
        {
            this.baseUri = baseUri;
            this.provider = provider;
            this.logger = logger;
        }

        public async Task<PersonalInfo> FindPersonalInfoAsync(string id)
        {
            return await Task.Run(() =>
            {
                return FindPersonalInfo(id);
            });            
        }

        private PersonalInfo FindPersonalInfo(string id)
        {
            var response = RequestPersonalInfo(id);
            if (response.IsSuccessful)
                return MapPersonalInfo(response);
            
            logger.LogDebug($"PersonalInfoExternalService: Can't find Personal Information from External Resolve API. Api Url: {response.ResponseUri}, Api Response Status: {response.StatusCode}");
            return null;
        }

        private IRestResponse RequestPersonalInfo(string id)
        {
            var client = new RestClient(BuildUri(id));
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {provider.GetJwt()}");

            return client.Execute(request);
        }

        private PersonalInfo MapPersonalInfo(IRestResponse response)
        {
            try
            {
                var responseDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Content);

                var initials = responseDictionary["initials"].ToString();
                var lastNameAtBirth = responseDictionary["lastNameAtBirth"].ToString();
                var lastNameAtBirthPrefix = responseDictionary["lastNameAtBirthPrefix"].ToString();
                var strBirthDate = responseDictionary["birthDate"];

                return new PersonalInfo()
                {
                    Initials = initials,
                    LastNameAtBirth = lastNameAtBirth,
                    LastNameAtBirthPrefix = lastNameAtBirthPrefix,
                    BirthDate = (DateTime) strBirthDate
                };
            }
            catch(Exception e)
            {
                logger.LogDebug($"PersonalInfoExternalService: Can't map Personal Information from External Resolve API. Api Url: {response.ResponseUri}, Api Response Content: {response.Content}, Exception: {e.Message}");
                return null;
            }
        }

        private Uri BuildUri(string id)
        {
            return new Uri(baseUri.Replace("{id}", id));
        }
    }
}
