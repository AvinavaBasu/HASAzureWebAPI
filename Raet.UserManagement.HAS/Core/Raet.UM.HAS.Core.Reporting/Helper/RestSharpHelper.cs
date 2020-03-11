using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Core.Domain;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Raet.UM.HAS.Core.Reporting.Helper
{
    public class RestSharpHelper : IRestSharp
    {

        private readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public List<T> GetList<T>(RestSharpParams settings)
        {
            if (settings.IsDeserializeCamelCaseFormat)
                return JsonConvert.DeserializeObject<List<T>>(Request(settings), JsonSerializerSettings);
            return JsonConvert.DeserializeObject<List<T>>(Request(settings));
        }

        public List<T> PostList<T>(RestSharpParams settings)
        {
            if (settings.IsDeserializeCamelCaseFormat)
                return JsonConvert.DeserializeObject<List<T>>(Request(settings), JsonSerializerSettings);
            return JsonConvert.DeserializeObject<List<T>>(Request(settings));
        }

        public T Get<T>(RestSharpParams settings)
        {
            if (settings.IsDeserializeCamelCaseFormat)
                return JsonConvert.DeserializeObject<T>(Request(settings), JsonSerializerSettings);
            return JsonConvert.DeserializeObject<T>(Request(settings));
        }

        public string Get(RestSharpParams settings) => Request(settings);

        public string Post(RestSharpParams settings) => Request(settings);

        public void Delete(RestSharpParams settings) => Request(settings);

        private string Request(RestSharpParams settings)
        {
            var response = GetRestResponse(settings);
            if (response.ErrorException != null)
                throw new Exception(
                    $"Error while fetching the data, Endpoint : {settings.ApiEndPoint}, ErrorException : {response.ErrorException}");
            return !response.IsSuccessful ? string.Empty : response.Content;
        }

        private IRestResponse GetRestResponse(RestSharpParams settings)
        {
            var endpointUri = $"{settings.BaseUrl}{settings.ApiEndPoint}";
            var client = new RestClient(settings.BaseUrl);
            var request = new RestRequest(settings.ApiEndPoint, settings.MethodType)
            {
                RequestFormat = settings.DataFormat,
            };

            // Add headers
            if (settings.Headers != null && settings.Headers.Any())
                foreach (var header in settings.Headers)
                    request.AddHeader(header.Key, header.Value);

            // Add parameters
            if (settings.Parameters != null && settings.Parameters.Any())
                foreach (var parameter in settings.Parameters)
                    request.AddParameter(parameter.Key, parameter.Value);

            // Add parameters using object
            if (settings.Parameter != null)
                request.AddJsonBody(settings.Parameter);

            var result = client.ExecuteTaskAsync(request).Result;
            return result;
        }
    }
}
