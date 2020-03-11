using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Mocks;

namespace Raet.UM.HAS.Infrastructure.Integration.Tests.Core.Reporting
{
    [TestClass]
    public class PersonalInfoEnrichmentServiceIntegrationTests
    {
        string _context = "Raet.UM.HAS.IntegrationTests";
                                
        [TestInitialize]
        public async Task Initialize()
        {
            await AzureTableStorageHelper.EnsureContextMappingIsCreated(_context, InfrastructureConfiguration.PortalExternalResolveUri);
        }

        [TestCleanup]
        public async Task CleanUp()
        {
            await AzureTableStorageHelper.CleanUpContextMapping(_context);
            await AzureTableStorageHelper.CleanUpPersonFromLocalStorage(_context, "ID10000");
            await AzureTableStorageHelper.CleanUpPersonFromLocalStorage(_context, "RP135685");
        }
        
        [TestMethod]
        public async Task ResolvePerson_PersonalInfoIsFoundInLocalStorage_ResolverReturnsTechnicalKeysAndEnrichedPersonalInfo()
        {
            string id = "ID10000";
            string initials = "LDY";
            string lastNameAtBirth = "Dominguez Yates";
            string lastNameAtBirthPrefix = "";
            DateTime birthDate = new DateTime(1989, 5, 24);

            await AzureTableStorageHelper.EnsurePersonIsInLocalStorage(_context, id, initials, lastNameAtBirth, lastNameAtBirthPrefix, birthDate);

            var logger = new MockInMemoryLogger();
            var service = PersonalInfoEnrichmentServiceHelper.BuildService(logger);

            var externalId = new ExternalId() { Context = _context, Id = id };
            var resolvedPerson = await service.ResolvePerson(externalId);

            Assert.AreEqual(_context, resolvedPerson.Key.Context);
            Assert.AreEqual(id, resolvedPerson.Key.Id);
            Assert.AreEqual(initials, resolvedPerson.PersonalInfo.Initials);
            Assert.AreEqual(lastNameAtBirth, resolvedPerson.PersonalInfo.LastNameAtBirth);
            Assert.AreEqual(lastNameAtBirthPrefix, resolvedPerson.PersonalInfo.LastNameAtBirthPrefix);
            Assert.AreEqual(birthDate, resolvedPerson.PersonalInfo.BirthDate);
            Assert.AreEqual(0, logger.Logs.Count);
        }

       // [TestMethod]
        public async Task ResolvePerson_PersonalInfoIsFoundInExternalApi_ResolverReturnsTechnicalKeysAndEnrichedPersonalInfo()
        {
            //  Warning: This test is tied to existing information being retrieved from the External Resolver Api
            string id = "RP135685";
            string initials = "J";
            string lastNameAtBirth = "Vries";
            string lastNameAtBirthPrefix = "V";
            DateTime birthDate = new DateTime(1969, 8, 5);
            
            await AzureTableStorageHelper.CleanUpPersonFromLocalStorage(_context, id);

            var logger = new MockInMemoryLogger();
            var service = PersonalInfoEnrichmentServiceHelper.BuildService(logger);
            var externalId = new ExternalId() { Context = _context, Id = id };
            var resolvedPerson = await service.ResolvePerson(externalId);

            Assert.AreEqual(_context, resolvedPerson.Key.Context);
            Assert.AreEqual(id, resolvedPerson.Key.Id);
            Assert.AreEqual(initials, resolvedPerson.PersonalInfo.Initials);
            Assert.AreEqual(lastNameAtBirth, resolvedPerson.PersonalInfo.LastNameAtBirth);
            Assert.AreEqual(lastNameAtBirthPrefix, resolvedPerson.PersonalInfo.LastNameAtBirthPrefix);
            Assert.AreEqual(birthDate, resolvedPerson.PersonalInfo.BirthDate);
            Assert.AreEqual(0, logger.Logs.Count);
        }

       // [TestMethod]
        public async Task ResolvePerson_PersonalInfoIsFoundInExternalApiButItHasNullBirthday_ResolverReturnsTechnicalKeysOnlyAndItLogsIssues()
        {
            //  Warning: This test is tied to existing information being retrieved from the External Resolver Api
            string idWithNullBirthday = "RU101080";
            
            await AzureTableStorageHelper.CleanUpPersonFromLocalStorage(_context, idWithNullBirthday);

            var logger = new MockInMemoryLogger();
            var service = PersonalInfoEnrichmentServiceHelper.BuildService(logger);
            var externalId = new ExternalId() { Context = _context, Id = idWithNullBirthday };
            var resolvedPerson = await service.ResolvePerson(externalId);
            
            Assert.AreEqual(_context, resolvedPerson.Key.Context);
            Assert.AreEqual(idWithNullBirthday, resolvedPerson.Key.Id);
            Assert.IsNull(resolvedPerson.PersonalInfo);
            
            Assert.AreEqual(2, logger.Logs.Count);

            var log = logger.Logs.Dequeue();
            Assert.AreEqual(LogLevel.Debug, log.LogLevel);
            Assert.IsTrue(log.Message.StartsWith("PersonalInfoExternalService: Can't map Personal Information from External Resolve API"));
            
            log = logger.Logs.Dequeue();
            Assert.AreEqual(LogLevel.Warning, log.LogLevel);
            Assert.AreEqual(
                $"PersonalInfoEnrichmentService: Error on getting Personal Info for ExternalId [Context: {_context}, Id: {idWithNullBirthday}]. Personal Info not found by provided resolver api for given context. Person will be created based on Technical Keys only.",
                log.Message);
        }

        [TestMethod]
        public async Task ResolvePerson_PersonalInfoNotFound_ResolverReturnsTechnicalKeysOnlyAndItLogsIssues()
        {
            string id = "unexistingId";
            string context = "Has.Users";
            var logger = new MockInMemoryLogger();
            var service = PersonalInfoEnrichmentServiceHelper.BuildService(logger);
            var externalId = new ExternalId() { Context = context, Id = id };
            var resolvedPerson = await service.ResolvePerson(externalId);

            Assert.AreEqual(context, resolvedPerson.Key.Context);
            Assert.AreEqual(id, resolvedPerson.Key.Id);
            Assert.IsNull(resolvedPerson.PersonalInfo);
            Assert.AreEqual(2, logger.Logs.Count);

            var log = logger.Logs.Dequeue();
            Assert.AreEqual(LogLevel.Debug, log.LogLevel);
            Assert.IsTrue(log.Message.StartsWith("PersonalInfoExternalService: Can't find Personal Information from External Resolve API"));

            log = logger.Logs.Dequeue();
            Assert.AreEqual(LogLevel.Warning, log.LogLevel);
            Assert.AreEqual(
                $"PersonalInfoEnrichmentService: Error on getting Personal Info for ExternalId [Context: {context}, Id: {id}]. Personal Info not found by provided resolver api for given context. Person will be created based on Technical Keys only.",
                log.Message);
        }

        [TestMethod]
        public async Task ResolvePerson_UknownContextMapping_ResolverReturnsTechnicalKeysOnlyAndItLogsIssues()
        {
            string uknownContext = "UknownContext";
            string id = "ID10000";

            var logger = new MockInMemoryLogger();
            var service = PersonalInfoEnrichmentServiceHelper.BuildService(logger);
            var externalId = new ExternalId() { Context = uknownContext, Id = id };
            var resolvedPerson = await service.ResolvePerson(externalId);

            Assert.AreEqual(uknownContext, resolvedPerson.Key.Context);
            Assert.AreEqual(id, resolvedPerson.Key.Id);
            Assert.IsNull(resolvedPerson.PersonalInfo);
            Assert.AreEqual(2, logger.Logs.Count);

            var log = logger.Logs.Dequeue();
            Assert.AreEqual(LogLevel.Debug, log.LogLevel);
            Assert.AreEqual(
                $"PersonalInfoExternalServiceFactory: Error in ContextMapping Table Storage, can't find a valid Url for given context {uknownContext}",
                log.Message);

            log = logger.Logs.Dequeue();
            Assert.AreEqual(LogLevel.Warning, log.LogLevel);
            Assert.AreEqual(
                $"PersonalInfoEnrichmentService: Error on getting Personal Info for ExternalId [Context: {uknownContext}, Id: {id}]. Can't find a valid resolver api for given context. Person will be created based on Technical Keys only.",
                log.Message);
        }
    }
}
