using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Infrastructure.Storage.Table;

namespace Raet.UM.HAS.Mocks
{
    public class MockPersonalLocalStorage : IPersonLocalStorage
    {
        private List<Person> persons;

        public MockPersonalLocalStorage()
        {
            //  Mock Data
            persons = new List<Person>()
            {
                new Person(
                    new ExternalId(){
                        Context = "Youforce",
                        Id = "IC000001"
                    },
                    new PersonalInfo()
                    {
                        Initials = "J", LastNameAtBirth = "Doe", BirthDate = new DateTime(2000,1,1)
                    })
            };
        }

        public Task<Person> CreatePersonAsync(Person person)
        {
            persons.Add(person);
            return Task.FromResult(person);
        }

        public Task<Person> FindPersonAsync(ExternalId externalId)
        {
            var person = persons.Find(x => x.Key.Context == externalId.Context &&
                                            x.Key.Id == externalId.Id);

            return Task.FromResult(person);
        }
    }
}
