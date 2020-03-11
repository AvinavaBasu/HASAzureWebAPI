using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting;
using Raet.UM.HAS.Mocks;
using System;
using System.Threading.Tasks;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Infrastructure.Storage.Table;

namespace Raet.UM.HAS.Core.Tests.Services
{
    [TestClass]
    public class PersonalInfoEnrichmentServiceTests
    {
        [TestMethod]
        public async Task ResolvePerson_ExternalIdFoundInLocalStorage_ReturnsPersonFromLocalStorage()
        {
            var externalId = new ExternalId() { Context = "Youforce", Id = "0001" };

            Mock<IPersonLocalStorage> localStorage = new Mock<IPersonLocalStorage>();
            localStorage.Setup(x => x.FindPersonAsync(externalId))
                .ReturnsAsync(new Person(externalId, new PersonalInfo() { LastNameAtBirth = "Van der Test", Initials = "VT", BirthDate = new DateTime(1970, 1, 15) }));

            var personalInfoEnrichmentService = 
                new PersonalInfoEnrichmentService(
                    localStorage.Object, 
                    new Mock<IPersonalInfoExternalServiceFactory>().Object, 
                    new Mock<ILogger>().Object);

            var person = await personalInfoEnrichmentService.ResolvePerson(externalId);

            Assert.IsNotNull(person);
            Assert.AreEqual(externalId.Context, person.Key.Context);
            Assert.AreEqual(externalId.Id, person.Key.Id);
            Assert.AreEqual("Van der Test", person.PersonalInfo.LastNameAtBirth);
            Assert.AreEqual("VT", person.PersonalInfo.Initials);
            Assert.AreEqual(1970, person.PersonalInfo.BirthDate.Year);
            Assert.AreEqual(1, person.PersonalInfo.BirthDate.Month);
            Assert.AreEqual(15, person.PersonalInfo.BirthDate.Day);
        }

        [TestMethod]
        public async Task ResolvePerson_ExternalIdNotFoundInLocalStorage_UnknownContext_ReturnsPersonWithoutEnrichmentInfo()
        {
            var externalId = new ExternalId() { Context = "Other", Id = "0001" };

            Mock<IPersonLocalStorage> localStorage = new Mock<IPersonLocalStorage>();
            localStorage.Setup(x => x.FindPersonAsync(externalId)).ReturnsAsync(() => null);

            Mock<IPersonalInfoExternalServiceFactory> extServiceFactory = new Mock<IPersonalInfoExternalServiceFactory>();
            extServiceFactory.Setup(x => x.Resolve("Other")).Returns(() => null);

            var logger = new MockInMemoryLogger();

            var personalInfoEnrichmentService = new PersonalInfoEnrichmentService(localStorage.Object, extServiceFactory.Object, logger);
            var person = await personalInfoEnrichmentService.ResolvePerson(externalId);
                       
            Assert.IsNotNull(person);
            Assert.AreEqual(externalId.Context, person.Key.Context);
            Assert.AreEqual(externalId.Id, person.Key.Id);
            Assert.IsNull(person.PersonalInfo);

            var log = logger.Logs.Dequeue();
            Assert.AreEqual(LogLevel.Warning, log.LogLevel);
            Assert.AreEqual(
                $"PersonalInfoEnrichmentService: Error on getting Personal Info for ExternalId [Context: {externalId.Context}, Id: {externalId.Id}]. Can't find a valid resolver api for given context. Person will be created based on Technical Keys only.",
                log.Message);
        }

        [TestMethod]
        public async Task ResolvePerson_ExternalIdNotFoundInLocalStorage_UnknownId_ReturnsPersonWithoutEnrichmentInfo()
        {
            var externalId = new ExternalId() { Context = "Youforce", Id = "0002" };

            Mock<IPersonLocalStorage> localStorage = new Mock<IPersonLocalStorage>();
            localStorage.Setup(x => x.FindPersonAsync(externalId)).ReturnsAsync(() => null);

            Mock<IPersonalInfoExternalService> externalService = new Mock<IPersonalInfoExternalService>();
            externalService.Setup(x => x.FindPersonalInfoAsync(externalId.Id)).ReturnsAsync(() => null);

            Mock<IPersonalInfoExternalServiceFactory> extServiceFactory = new Mock<IPersonalInfoExternalServiceFactory>();
            extServiceFactory.Setup(x => x.Resolve("Youforce")).Returns(externalService.Object);

            var logger = new MockInMemoryLogger();

            var personalInfoEnrichmentService = new PersonalInfoEnrichmentService(localStorage.Object, extServiceFactory.Object, logger);
            var person = await personalInfoEnrichmentService.ResolvePerson(externalId);

            Assert.IsNotNull(person);
            Assert.AreEqual(externalId.Context, person.Key.Context);
            Assert.AreEqual(externalId.Id, person.Key.Id);
            Assert.IsNull(person.PersonalInfo);

            var log = logger.Logs.Dequeue();
            Assert.AreEqual(LogLevel.Warning, log.LogLevel);
            Assert.AreEqual(
                $"PersonalInfoEnrichmentService: Error on getting Personal Info for ExternalId [Context: {externalId.Context}, Id: {externalId.Id}]. Personal Info not found by provided resolver api for given context. Person will be created based on Technical Keys only.",
                log.Message);
        }

        [TestMethod]
        public async Task ResolvePerson_ExternalIdNotFoundInLocalStorage_ExternalPersonalInformationFound_ReturnsPersonCreatedWithExternalPersonalInformation()
        {
            var externalId = new ExternalId() { Context = "Youforce", Id = "0002" };

            Mock<IPersonLocalStorage> localStorage = new Mock<IPersonLocalStorage>();
            localStorage.Setup(x => x.FindPersonAsync(externalId)).ReturnsAsync(() => null);
            localStorage.Setup(x => x.CreatePersonAsync(It.IsAny<Person>())).ReturnsAsync((Person newPerson) => newPerson);

            Mock<IPersonalInfoExternalService> externalService = new Mock<IPersonalInfoExternalService>();
            externalService.Setup(x => x.FindPersonalInfoAsync(externalId.Id))
                .ReturnsAsync(new PersonalInfo() { LastNameAtBirth = "Ext Test User", Initials = "ETU", BirthDate = new DateTime(1980, 5, 21) });

            Mock<IPersonalInfoExternalServiceFactory> extServiceFactory = new Mock<IPersonalInfoExternalServiceFactory>();
            extServiceFactory.Setup(x => x.Resolve("Youforce")).Returns(externalService.Object);

            var personalInfoEnrichmentService = new PersonalInfoEnrichmentService(localStorage.Object, extServiceFactory.Object, new Mock<ILogger>().Object);
            var person = await personalInfoEnrichmentService.ResolvePerson(externalId);

            Assert.IsNotNull(person);
            Assert.AreEqual(externalId.Context, person.Key.Context);
            Assert.AreEqual(externalId.Id, person.Key.Id);
            Assert.AreEqual("Ext Test User", person.PersonalInfo.LastNameAtBirth);
            Assert.AreEqual("ETU", person.PersonalInfo.Initials);
            Assert.AreEqual(1980, person.PersonalInfo.BirthDate.Year);
            Assert.AreEqual(5, person.PersonalInfo.BirthDate.Month);
            Assert.AreEqual(21, person.PersonalInfo.BirthDate.Day);
        }

        [TestMethod]
        public async Task ResolvePerson_NullExternalId_ThrowsException()
        {
            MockInMemoryLogger logger = new MockInMemoryLogger();
            try
            {
                var personalInfoEnrichmentService =
                    new PersonalInfoEnrichmentService(
                        new Mock<IPersonLocalStorage>().Object,
                        new Mock<IPersonalInfoExternalServiceFactory>().Object,
                        logger);

                await personalInfoEnrichmentService.ResolvePerson(null);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(ArgumentException), e.GetType());
            }
            finally
            {
                var log = logger.Logs.Dequeue();
                Assert.AreEqual(LogLevel.Error, log.LogLevel);
                Assert.AreEqual("PersonalInfoEnrichmentService: Invalid External ID", log.Message);
            }
        }

        [TestMethod]
        public async Task ResolvePerson_EmptyContext_ThrowsException()
        {
            MockInMemoryLogger logger = new MockInMemoryLogger();
            try
            {
                var personalInfoEnrichmentService =
                    new PersonalInfoEnrichmentService(
                        new Mock<IPersonLocalStorage>().Object,
                        new Mock<IPersonalInfoExternalServiceFactory>().Object,
                        logger);

                var externalId = new ExternalId() { Id = "0001" };
                await personalInfoEnrichmentService.ResolvePerson(externalId);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(ArgumentException), e.GetType());
            }
            finally
            {
                var log = logger.Logs.Dequeue();
                Assert.AreEqual(LogLevel.Error, log.LogLevel);
                Assert.AreEqual("PersonalInfoEnrichmentService: Invalid External ID", log.Message);
            }
        }

        [TestMethod]
        public async Task ResolvePerson_EmptyId_ThrowsException()
        {
            MockInMemoryLogger logger = new MockInMemoryLogger();
            try
            {
                var personalInfoEnrichmentService =
                new PersonalInfoEnrichmentService(
                    new Mock<IPersonLocalStorage>().Object,
                    new Mock<IPersonalInfoExternalServiceFactory>().Object,
                    logger);

                var externalId = new ExternalId() { Context = "Youforce" };
                await personalInfoEnrichmentService.ResolvePerson(externalId);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(ArgumentException), e.GetType());
            }
            finally
            {
                var log = logger.Logs.Dequeue();
                Assert.AreEqual(LogLevel.Error, log.LogLevel);
                Assert.AreEqual("PersonalInfoEnrichmentService: Invalid External ID", log.Message);
            }
        }
    }
}
