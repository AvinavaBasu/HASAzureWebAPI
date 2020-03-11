using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Core.Reporting.Business;
using Xunit;
using Raet.UM.HAS.Infrastructure.Storage.Table;

namespace Raet.UM.HAS.Core.Reporting.Tests
{
    public class UserInformationTest
    {

        public Mock<IPersonLocalStorage> PersonLocalStorageMock { get; set; }
        public Mock<IContextMappingLocalStorage> ContextMappingLocalStorage { get; set; }
        public Mock<IRestSharp> RestSharpHelper { get; set; }
        public ExternalId ExternalIdMockInfoUser1 { get; set; }
        public ExternalId ExternalIdMockInfoUser2 { get; set; }

        public UserInformationTest()
        {
            PersonLocalStorageMock = new Mock<IPersonLocalStorage>();
            ContextMappingLocalStorage = new Mock<IContextMappingLocalStorage>();
            RestSharpHelper = new Mock<IRestSharp>();
            ExternalIdMockInfoUser1 = new ExternalId {Context = "Youforce.Users", Id = "IC12345"};
            ExternalIdMockInfoUser2 = new ExternalId {Context = "Youforce.Users", Id = "IC54321"};
        }

        [Fact]
        public void WhenThereisNoPersonInfoInStoredTable_thenShouldCallTheAPIForGettingTheUserInfo()
        {

            PersonLocalStorageMock.Setup(x => x.FindPersonAsync(It.IsAny<ExternalId>()))
                .Returns(Task.FromResult<Person>(null));
            ContextMappingLocalStorage.Setup(x => x.Resolve(It.IsAny<string>())).Returns(
                "http://we-d-app-youforce-ext-resolver.azurewebsites.net/api/persons/{id}/personaldetails");
            RestSharpHelper.Setup(e => e.Get<Person>(It.IsAny<RestSharpParams>())).Returns(GetRandomPersonData());
            var userInfo = new UserInformation(PersonLocalStorageMock.Object, ContextMappingLocalStorage.Object,
                RestSharpHelper.Object);
            var result = userInfo.GetUsers(new List<ExternalId>
            {
                ExternalIdMockInfoUser1,
                ExternalIdMockInfoUser2
            });
            Assert.True(result.ToList().Count == 2);
        }

        [Fact]
        public void WhenThereisValidPersonInfoInStoredTable_thenShouldFetchTheDataFromTheUserInfoTable()
        {

            var personalInfoMock = new PersonalInfo
                {BirthDate = DateTime.Today, Initials = "S", LastNameAtBirth = "A", LastNameAtBirthPrefix = "B"};
            PersonLocalStorageMock.Setup(x => x.FindPersonAsync(It.Is<ExternalId>(y => y.Id == "IC12345")))
                .Returns(Task.FromResult(new Person(ExternalIdMockInfoUser1, personalInfoMock)));
            var userInfo = new UserInformation(PersonLocalStorageMock.Object, ContextMappingLocalStorage.Object, RestSharpHelper.Object);
            var result = userInfo.GetUsers(new List<ExternalId>
            {
                ExternalIdMockInfoUser1
            });
            Assert.True(result.ToList().Count == 1);
            Assert.Equal(result.First().PersonalInfo.Initials, personalInfoMock.Initials);
            Assert.Equal(result.First().PersonalInfo.LastNameAtBirth, personalInfoMock.LastNameAtBirth);
            Assert.Equal(result.First().PersonalInfo.LastNameAtBirthPrefix, personalInfoMock.LastNameAtBirthPrefix);
        }

        private Person GetRandomPersonData()
        {
            var personalInfoMock = new PersonalInfo
                {BirthDate = DateTime.Today, Initials = "S", LastNameAtBirth = "A", LastNameAtBirthPrefix = "B"};
            return new Person(ExternalIdMockInfoUser1, personalInfoMock);
        }
    }
}
