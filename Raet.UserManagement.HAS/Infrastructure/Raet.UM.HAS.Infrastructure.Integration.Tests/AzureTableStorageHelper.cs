using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Raet.UM.HAS.Infrastructure.Storage.Table;
using Raet.UM.HAS.Infrastructure.Storage.Table.Model;

namespace Raet.UM.HAS.Infrastructure.Integration.Tests
{
    public static class AzureTableStorageHelper
    {
        public static IAzureTableStorageInitializer InitializeTableStorage()
        {
            var settings = new TableStorageSettings()
            {
                ConnectionString = InfrastructureConfiguration.TableStorageConnString
            };
            return new AzureTableStorageInitializer(settings);
        }

        public static async Task CleanUpContextMapping(string context)
        {
            var initializer = InitializeTableStorage();
            var repo = new AzureTableStorageRepository<ContextMapping>(initializer, new Mock<ILogger>().Object);

            var mapping = await repo.RetrieveRecord("PersonInfo", context);
            if (mapping != null)
                await repo.DeleteRecord(mapping);
        }

        public static async Task EnsureContextMappingIsCreated(string context, string url)
        {
            var initializer = InitializeTableStorage();
            var repo = new AzureTableStorageRepository<ContextMapping>(initializer, new Mock<ILogger>().Object);

            var mapping = await repo.RetrieveRecord("PersonInfo", context);
            if (mapping != null)
                await repo.DeleteRecord(mapping);
                       
            await repo.InsertRecordToTable(new ContextMapping()
            {
                PartitionKey = "PersonInfo",
                RowKey = context,
                URL = url
            });

        }

        public static async Task CleanUpPersonFromLocalStorage(string context, string id)
        {
            var initializer = InitializeTableStorage();
            var repo = new AzureTableStorageRepository<StoredPersonalInfo>(initializer, new Mock<ILogger>().Object);

            var person = await repo.RetrieveRecord(context, id);
            if (person != null)
                await repo.DeleteRecord(person);
        }

        public static async Task EnsurePersonIsInLocalStorage(string context, string id, string initials, string lastNameAtBirth, string lastNameAtBirthPrefix, DateTime birthdate)
        {
            var initializer = InitializeTableStorage();
            var repo = new AzureTableStorageRepository<StoredPersonalInfo>(initializer, new Mock<ILogger>().Object);

            var person = await repo.RetrieveRecord(context, id);
            if (person != null)
                await repo.DeleteRecord(person);

            await repo.InsertRecordToTable(new StoredPersonalInfo()
            {
                PartitionKey = context,
                RowKey = id,
                initials = initials,
                lastNameAtBirth = lastNameAtBirth,
                lastNameAtBirthPrefix = lastNameAtBirthPrefix,
                birthdate = birthdate.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("nl"))
            });
        }
    }
}
