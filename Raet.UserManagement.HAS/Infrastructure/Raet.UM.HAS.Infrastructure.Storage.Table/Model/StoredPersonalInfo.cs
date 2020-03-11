using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;
using Raet.UM.HAS.Core.Domain;

namespace Raet.UM.HAS.Infrastructure.Storage.Table.Model
{
    public class StoredPersonalInfo : TableEntity
    {
        
        public string initials { get; set; }

        public string lastNameAtBirth { get; set; }

        public string lastNameAtBirthPrefix { get; set; }

        public string birthdate { get; set; }

        public StoredPersonalInfo()
        {

        }

        public StoredPersonalInfo( Person person) 
        {
            PartitionKey = person.Key.Context;
            RowKey = person.Key.Id;
            initials = person.PersonalInfo.Initials;
            lastNameAtBirthPrefix = person.PersonalInfo.LastNameAtBirthPrefix;
            lastNameAtBirth = person.PersonalInfo.LastNameAtBirth;
            birthdate = person.PersonalInfo.BirthDate.ToString();
        }
    }
}
