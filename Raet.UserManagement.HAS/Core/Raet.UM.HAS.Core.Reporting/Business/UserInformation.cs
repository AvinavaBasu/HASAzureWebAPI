using System;
using Raet.UM.HAS.Core.Domain;
using System.Collections.Generic;
using RestSharp;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Infrastructure.Storage.Table;

namespace Raet.UM.HAS.Core.Reporting.Business
{
    public class UserInformation : IUserInformation
    {
        private IPersonLocalStorage PersonLocalStorage { get; set; }
        public IContextMappingLocalStorage ContextMappingLocalStorage { get; set; }
        public IRestSharp RestSharpHelper { get; set; }

        public UserInformation(IPersonLocalStorage personLocalStorage,
            IContextMappingLocalStorage contextMappingLocalStorage,
            IRestSharp restSharpHelper)
        {
            PersonLocalStorage = personLocalStorage;
            ContextMappingLocalStorage = contextMappingLocalStorage;
            RestSharpHelper = restSharpHelper;
        }

        public IEnumerable<Person> GetUsers(IEnumerable<ExternalId> userIds)
        {
            var personInfo = new List<Person>();
            var contextEndPoint = new Dictionary<string, string>();
            foreach (var user in userIds)
            {
                var person = PersonLocalStorage.FindPersonAsync(user).Result;
                personInfo.Add(person ?? GetUserByContext(ref contextEndPoint, user));
            }

            return personInfo;
        }

        private Person GetUserByContext(ref Dictionary<string, string> contextMapping, ExternalId user)
        {
            var urlEndPoint = contextMapping.ContainsKey(user.Context)
                ? contextMapping[user.Context]
                : ContextMappingLocalStorage.Resolve(user.Context);

            if (urlEndPoint == null) return GetPersonWithNoPersonInfo(user);

            if (!contextMapping.ContainsKey(user.Context))
                contextMapping.Add(user.Context, urlEndPoint);

            var personInfo = RestSharpHelper.Get<PersonalInfo>(new RestSharpParams()
            {
                ApiEndPoint = urlEndPoint.Replace("{id}", user.Id),
                BaseUrl = GetBaseUri(urlEndPoint),
                MethodType = Method.GET
            });
            var person = PersonLocalStorage.CreatePersonAsync(new Person(user, personInfo)).Result;
            return person;
        }

        private static Person GetPersonWithNoPersonInfo(ExternalId key)
        {
            return new Person(key, new PersonalInfo());
        }

        private static string GetBaseUri(string url)
        {
            var uri = new Uri(url);
            return uri.GetLeftPart(UriPartial.Authority);
        }
    }
}
