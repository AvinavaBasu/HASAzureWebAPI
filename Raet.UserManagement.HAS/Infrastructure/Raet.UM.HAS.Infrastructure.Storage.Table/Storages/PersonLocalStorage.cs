using Raet.UM.HAS.Infrastructure.Storage.Table.Model;
using System;
using System.Threading.Tasks;
using Raet.UM.HAS.Core.Domain;
using System.Globalization;

namespace Raet.UM.HAS.Infrastructure.Storage.Table
{
    public class PersonLocalStorage : IPersonLocalStorage
    {
        IAzureTableStorageRepository<StoredPersonalInfo> _repo;

        public PersonLocalStorage(IAzureTableStorageRepository<StoredPersonalInfo> repo)
        {
            _repo = repo;
        }

        public async Task<Person> CreatePersonAsync(Person person)
        {
            var result = await _repo.InsertRecordToTable(new StoredPersonalInfo(person));
            if (result == null) return null;
            return person;
        }

        public async Task<Person> FindPersonAsync(ExternalId externalId)
        {
            var persInfo = await _repo.RetrieveRecord(externalId.Context, externalId.Id);
            if (persInfo == null) return null;
            //(dd/MM/YYYY)Datetime format conversion throws error- need to fix this here
            //OR
            //validate the date while populating the data in the tbl storage 
            return new Person(externalId, new PersonalInfo
            {
                Initials = persInfo.initials,
                LastNameAtBirth = persInfo.lastNameAtBirth,
                LastNameAtBirthPrefix = persInfo.lastNameAtBirthPrefix,
                BirthDate = DateTime.Parse(persInfo.birthdate, CultureInfo.CreateSpecificCulture("nl"))
            });

        }
    }
}
