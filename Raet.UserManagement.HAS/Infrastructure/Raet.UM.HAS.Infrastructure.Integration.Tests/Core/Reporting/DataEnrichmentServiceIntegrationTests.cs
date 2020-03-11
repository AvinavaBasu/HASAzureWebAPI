using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting;
using Raet.UM.HAS.Infrastructure.Storage.CosmosDB;
using Raet.UM.HAS.Mocks;

namespace Raet.UM.HAS.Infrastructure.Integration.Tests.Core.Reporting
{
    [TestClass]
    public class DataEnrichmentServiceIntegrationTests
    {
        string _tenantId = "DataEnrichmentServiceIntegrationTests";
        string _context = "Raet.UM.HAS.IntegrationTests";
        
        string _userId = "ID10000";
        PersonalInfo _userPersonalInfo = new PersonalInfo()
        {
            Initials = "LDY",
            LastNameAtBirth = "Dominguez Yates",
            LastNameAtBirthPrefix ="",
            BirthDate = new DateTime(1989, 5, 24)
        };

        string _targetId = "RP135685";
        PersonalInfo _targetPersonalInfo = new PersonalInfo()
        {
            Initials = "J",
            LastNameAtBirth = "Vries",
            LastNameAtBirthPrefix = "V",
            BirthDate = new DateTime(1969, 8, 5)
        };

        DateTime _from = new DateTime(2018, 1, 1).ToUniversalTime();
        DateTime _until = new DateTime(2018, 5, 1).ToUniversalTime();

        RawEventStorageRepository _repository;
        List<string> _eventIdList = new List<string>();

        [TestInitialize]
        public async Task Initialize()
        {
            Mock<ILoggerFactory> factory = new Mock<ILoggerFactory>();
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
            var config = builder.Build();
            var _dbconfig = config.GetSection("CosmoDBSettings").Get<CosmoDBSettings>();

            _repository = new RawEventStorageRepository(new CosmoDBStorageInitializer(_dbconfig), factory.Object);

            await AzureTableStorageHelper.EnsurePersonIsInLocalStorage(_context, _userId, _userPersonalInfo.Initials, _userPersonalInfo.LastNameAtBirth, _userPersonalInfo.LastNameAtBirthPrefix, _userPersonalInfo.BirthDate);
            await AzureTableStorageHelper.EnsurePersonIsInLocalStorage(_context, _targetId, _targetPersonalInfo.Initials, _targetPersonalInfo.LastNameAtBirth, _targetPersonalInfo.LastNameAtBirthPrefix, _targetPersonalInfo.BirthDate);
        }

        [TestCleanup]
        public async Task CleanUp()
        {
            foreach (string eventId in _eventIdList)
            {
                await _repository.DeleteRawEventAsync(eventId, new RequestOptions() { PartitionKey = new Microsoft.Azure.Documents.PartitionKey(Microsoft.Azure.Documents.Undefined.Value) });
            }

            _eventIdList.Clear();

            await AzureTableStorageHelper.CleanUpPersonFromLocalStorage(_context, _userId);
            await AzureTableStorageHelper.CleanUpPersonFromLocalStorage(_context, _targetId);
        }

        [TestMethod]
        public async Task AddEffectiveAuthorizationAsync_GrantedEventForExistingUserAndTarget_OpenIntervalIsCreatedInReadModelWithEnrichedPersonalInfo()
        {
            //Create a raw event
            var user = new ExternalId() { Context = _context, Id = _userId };
            var permission = new Permission() { Application = "YOUFORCE", Id = "HomeAccess", Description = "Home Access" };
            var target = new ExternalId() { Id = _targetId, Context = _context };
            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = _tenantId, Permission = permission, User = user, Target = target };
            var eaEvent = new EffectiveAuthorizationGrantedEvent() { From = _from, EffectiveAuthorization = effectiveAuthorization, DateCreated = DateTime.Now };
            
            //Add event to the memory event store
            _eventIdList.Add(await _repository.WriteRawEventAsync(eaEvent));
            
            //Data Enrichment
            var eaHandlerFactory = new EffectiveAuthorizationHandlerFactory();
            eaHandlerFactory.RegisterHandler(typeof(EffectiveAuthorizationGrantedEvent), new PermissionGrantedHandler());
            var eatimelineFactory = new EffectiveAuthorizationTimelineFactory(_repository, eaHandlerFactory);

            var logger = new MockInMemoryLogger();
            var prService = PersonalInfoEnrichmentServiceHelper.BuildService(logger);
            var readModel = new ReportingInMemoryStorage();

            var dataEnrichmentService = new DataEnrichmentService(eatimelineFactory, prService, readModel);

            await dataEnrichmentService.AddEffectiveAuthorizationAsync(eaEvent);

            //Assertions
            var expectedUser = new Person(user, _userPersonalInfo);
            var expectedTarget = new Person(target, _targetPersonalInfo);
            var intervals = readModel.GetIntervals(effectiveAuthorization).Result;

            Assert.AreEqual(1, intervals.Count);
            Assert.AreEqual(_tenantId, intervals[0].TenantId);
            Assert.AreEqual(expectedUser, intervals[0].User);
            Assert.AreEqual(permission, intervals[0].Permission);
            Assert.AreEqual(expectedTarget, intervals[0].TargetPerson);
            Assert.AreEqual(_from, intervals[0].EffectiveInterval.Start);
        }

        [TestMethod]
        public async Task AddEffectiveAuthorizationAsync_GrantedAndRevokedEventsForExistingUserAndTarget_ClosedIntervalIsCreatedInReadModelWithEnrichedPersonalInfo()
        {
            //Create a raw event
            var user = new ExternalId() { Context = _context, Id = _userId };
            var permission = new Permission() { Application = "YOUFORCE", Id = "HomeAccess", Description = "Home Access" };
            var target = new ExternalId() { Id = _targetId, Context = _context };
            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = _tenantId, Permission = permission, User = user, Target = target };
            var eaGrantedEvent = new EffectiveAuthorizationGrantedEvent() { From = _from, EffectiveAuthorization = effectiveAuthorization, DateCreated = DateTime.Now };
            var eaRevokedEvent = new EffectiveAuthorizationRevokedEvent() { Until = _until, EffectiveAuthorization = effectiveAuthorization, DateCreated = DateTime.Now };


            _eventIdList.Add(await _repository.WriteRawEventAsync(eaGrantedEvent));
            _eventIdList.Add(await _repository.WriteRawEventAsync(eaRevokedEvent));
           

            //Data Enrichment
            var eaHandlerFactory = new EffectiveAuthorizationHandlerFactory();
            eaHandlerFactory.RegisterHandler(typeof(EffectiveAuthorizationGrantedEvent), new PermissionGrantedHandler());
            eaHandlerFactory.RegisterHandler(typeof(EffectiveAuthorizationRevokedEvent), new PermissionRevokedHandler());

            var eatimelineFactory = new EffectiveAuthorizationTimelineFactory(_repository, eaHandlerFactory);

            var logger = new MockInMemoryLogger();
            var prService = PersonalInfoEnrichmentServiceHelper.BuildService(logger);
            var readModel = new ReportingInMemoryStorage();

            var dataEnrichmentService = new DataEnrichmentService(eatimelineFactory, prService, readModel);

            await dataEnrichmentService.AddEffectiveAuthorizationAsync(eaRevokedEvent);

            //Assertions
            var expectedUser = new Person(user, _userPersonalInfo);
            var expectedTarget = new Person(target, _targetPersonalInfo);
            var intervals = readModel.GetIntervals(effectiveAuthorization).Result;

            Assert.AreEqual(1, intervals.Count);
            Assert.AreEqual(_tenantId, intervals[0].TenantId);
            Assert.AreEqual(expectedUser, intervals[0].User);
            Assert.AreEqual(permission, intervals[0].Permission);
            Assert.AreEqual(expectedTarget, intervals[0].TargetPerson);
            Assert.AreEqual(_from, intervals[0].EffectiveInterval.Start);
            Assert.AreEqual(_until, intervals[0].EffectiveInterval.End);
        }

        [TestMethod]
        public async Task AddEffectiveAuthorizationAsync_GrantedEventForUnexistingUserId_OpenIntervalIsCreatedInReadModelWithoutErichedPersonalInfo()
        {
            //Create a raw event
            var unexistingUser = new ExternalId() { Context = _context, Id = "unexistingUserId" };
            var permission = new Permission() { Application = "YOUFORCE", Id = "HomeAccess", Description = "Home Access" };
            var target = new ExternalId() { Id = _targetId, Context = _context };
            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = _tenantId, Permission = permission, User = unexistingUser, Target = target };
            var eaEvent = new EffectiveAuthorizationGrantedEvent() { From = _from, EffectiveAuthorization = effectiveAuthorization, DateCreated = DateTime.Now };

            _eventIdList.Add(await _repository.WriteRawEventAsync(eaEvent));

            //Data Enrichment
            var eaHandlerFactory = new EffectiveAuthorizationHandlerFactory();
            eaHandlerFactory.RegisterHandler(typeof(EffectiveAuthorizationGrantedEvent), new PermissionGrantedHandler());
            var eatimelineFactory = new EffectiveAuthorizationTimelineFactory(_repository, eaHandlerFactory);

            var logger = new MockInMemoryLogger();
            var prService = PersonalInfoEnrichmentServiceHelper.BuildService(logger);
            var readModel = new ReportingInMemoryStorage();

            var dataEnrichmentService = new DataEnrichmentService(eatimelineFactory, prService, readModel);

            await dataEnrichmentService.AddEffectiveAuthorizationAsync(eaEvent);

            //Assertions
            var expectedUser = new Person(unexistingUser, null);
            var expectedTarget = new Person(target, _targetPersonalInfo);
            var intervals = readModel.GetIntervals(effectiveAuthorization).Result;

            Assert.AreEqual(1, intervals.Count);
            Assert.AreEqual(_tenantId, intervals[0].TenantId);
            Assert.AreEqual(expectedUser.Key, intervals[0].User.Key);
            Assert.IsNull(intervals[0].User.PersonalInfo);
            Assert.AreEqual(permission, intervals[0].Permission);
            Assert.AreEqual(expectedTarget, intervals[0].TargetPerson);
            Assert.AreEqual(_from, intervals[0].EffectiveInterval.Start);
        }
        
        [TestMethod]
        public async Task AddEffectiveAuthorizationAsync_GrantedAndRevokedEventsForUnexistingUserContextAndWithoutTarget_ClosedIntervalIsCreatedInReadModelWithoutEnrichedPersonalInfo()
        {
            //Create a raw event
            var user = new ExternalId() { Context = "unexistingContext", Id = _userId };
            var permission = new Permission() { Application = "YOUFORCE", Id = "HomeAccess", Description = "" };
            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = _tenantId, Permission = permission, User = user };

            var eaGrantedEvent = new EffectiveAuthorizationGrantedEvent() { From = _from, EffectiveAuthorization = effectiveAuthorization, DateCreated = DateTime.Now };
            var eaRevokedEvent = new EffectiveAuthorizationRevokedEvent() { Until = _until, EffectiveAuthorization = effectiveAuthorization, DateCreated = DateTime.Now };


            _eventIdList.Add(await _repository.WriteRawEventAsync(eaGrantedEvent));
            _eventIdList.Add(await _repository.WriteRawEventAsync(eaRevokedEvent));

            //Data Enrichment
            var eaHandlerFactory = new EffectiveAuthorizationHandlerFactory();
            eaHandlerFactory.RegisterHandler(typeof(EffectiveAuthorizationRevokedEvent), new PermissionRevokedHandler());
            eaHandlerFactory.RegisterHandler(typeof(EffectiveAuthorizationGrantedEvent), new PermissionGrantedHandler());
            var eatimelineFactory = new EffectiveAuthorizationTimelineFactory(_repository, eaHandlerFactory);

            var logger = new MockInMemoryLogger();
            var prService = PersonalInfoEnrichmentServiceHelper.BuildService(logger);
            var readModel = new ReportingInMemoryStorage();

            var dataEnrichmentService = new DataEnrichmentService(eatimelineFactory, prService, readModel);

            await dataEnrichmentService.AddEffectiveAuthorizationAsync(eaRevokedEvent);

            //Assertions
            var expectedUser = new Person(user, null);
            var intervals = readModel.GetIntervals(effectiveAuthorization).Result;

            Assert.AreEqual(1, intervals.Count);
            Assert.AreEqual(_tenantId, intervals[0].TenantId);
            Assert.AreEqual(user, intervals[0].User.Key);
            Assert.IsNull(intervals[0].User.PersonalInfo);
            Assert.AreEqual(permission, intervals[0].Permission);
            Assert.AreEqual(null, intervals[0].TargetPerson);
            Assert.AreEqual(_from, intervals[0].EffectiveInterval.Start);
            Assert.AreEqual(_until, intervals[0].EffectiveInterval.End);
        }
    }
}
