using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Infrastructure.Storage.Table;
using Raet.UM.HAS.Infrastructure.Storage.Table.Model;

namespace Raet.UM.HAS.Infrastructure.Tests.TableStorage
{
    [TestClass]
    public class PersonLocalStorageTest
    {
        private Mock<IAzureTableStorageRepository<StoredPersonalInfo>> _azureTableStorageRepoMock;

        private PersonLocalStorage _personLocalStorage;

        public PersonLocalStorageTest()
        {
            _azureTableStorageRepoMock = new Mock<IAzureTableStorageRepository<StoredPersonalInfo>>();
        }

        [TestMethod]
        public void CreatePersonAsync_Test()
        {
            //Arrange
            _personLocalStorage = new PersonLocalStorage(_azureTableStorageRepoMock.Object);
            Person person = new Person(new ExternalId(), new PersonalInfo());

            _azureTableStorageRepoMock.Setup(e => e.InsertRecordToTable(It.IsAny<StoredPersonalInfo>()))
                .Returns(Task.FromResult(new TableResult()));

            //Act
            var result = _personLocalStorage.CreatePersonAsync(person);

            //Assert
            Assert.AreEqual("RanToCompletion", result.Status.ToString());
        }

    }
}
