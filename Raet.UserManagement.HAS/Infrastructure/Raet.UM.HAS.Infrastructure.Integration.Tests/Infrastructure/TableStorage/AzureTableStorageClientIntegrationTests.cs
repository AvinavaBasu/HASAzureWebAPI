using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using Raet.UM.HAS.Infrastructure.Storage.Table;
using Raet.UM.HAS.Infrastructure.Storage.Table.Model;

namespace Raet.UM.HAS.Infrastructure.Integration.Tests.Infrastructure.TableStorage
{
    [TestClass]
    public class AzureTableStorageClientIntegrationTests
    {
        [TestMethod]
        public async Task RepositoryIsAbleToConnectAndManageContextMappings()
        {
            var initializer = AzureTableStorageHelper.InitializeTableStorage();
            var repo = new AzureTableStorageRepository<ContextMapping>(initializer, new Mock<ILogger>().Object);

            var partitionKey = "PersonInfo";
            var rowKey = "AzureTableStorageClientIntegrationTests";

            //  Clean up
            var sut = await repo.RetrieveRecord(partitionKey, rowKey);
            if (sut != null)
                await repo.DeleteRecord(sut);

            //  Insert
            await repo.InsertRecordToTable(new ContextMapping()
            {
                PartitionKey = partitionKey,
                RowKey = rowKey,
                URL = "http://localhost:9002/youforcereseolver"
            });

            sut = await repo.RetrieveRecord(partitionKey, rowKey);
            Assert.IsNotNull(sut);
            Assert.IsInstanceOfType(sut, typeof(ContextMapping));
            Assert.IsFalse(string.IsNullOrEmpty(sut.URL));

            //  Delete
            await repo.DeleteRecord(sut);

            sut = await repo.RetrieveRecord(partitionKey, rowKey);
            Assert.IsNull(sut);
        }
        
        [TestMethod]
        public async Task RepositoryIsAbleToConnectAndManagePersonalInfo()
        {
            var initializer = AzureTableStorageHelper.InitializeTableStorage();
            var repo = new AzureTableStorageRepository<StoredPersonalInfo>(initializer, new Mock<ILogger>().Object);

            var partitionKey = "AzureTableStorageClientIntegrationTests";
            var rowKey = "R234568";

            //  Clean up
            var sut = await repo.RetrieveRecord(partitionKey, rowKey);
            if (sut != null)
                await repo.DeleteRecord(sut);

            //  Insert
            await repo.InsertRecordToTable(new StoredPersonalInfo()
            {
                PartitionKey = partitionKey,
                RowKey = rowKey,
                initials = "AP",
                lastNameAtBirthPrefix = "Aylen Perez",
                lastNameAtBirth = "",
                birthdate = "28/09/1976"
            });

            sut = await repo.RetrieveRecord(partitionKey, rowKey);
            Assert.IsNotNull(sut);
            Assert.IsInstanceOfType(sut, typeof(StoredPersonalInfo));

            //  Delete
            await repo.DeleteRecord(sut);

            sut = await repo.RetrieveRecord(partitionKey, rowKey);
            Assert.IsNull(sut);
        }

    }
}
