using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Infrastructure.Storage.CosmosDB;

namespace Raet.UM.HAS.Infrastructure.Integration.Tests.Infrastructure.CosmosDB
{
    [TestClass]
    public class ReportingStorageRepositoryIntegrationTests
    {
        ReportingStorageRepository _repository;

        readonly string _tenantId = "ReportingStorageRepositoryIntegrationTests";

        readonly string _ownerContext = "Youforce.Users";
        readonly string _ownerId = "ID00007";
        readonly string _targetId = "ID0009";

        readonly string _applicationName = "YOUFORCE";
        readonly string _Identifier = "HomeAccess";

        readonly DateTime _dateTimeStart1 = new DateTime(2018, 1, 1);
        readonly DateTime _dateTimeEnd1 = new DateTime(2018, 1, 2);

        readonly DateTime _birthDate1 = new DateTime(1930, 8, 25);
        readonly string _lastNameAtBirth1 = "Connery";
        readonly string _lastNameAtBirthPrefix1 = "Mr";
        readonly string _initials1 = "SC";

        readonly DateTime _birthDate2 = new DateTime(1945, 7, 30);
        readonly string _lastNameAtBirth2 = "Smith";
        readonly string _lastNameAtBirthPrefix2 = "Mr";
        readonly string _initials2 = "PS";

        readonly List<string> _idToDeleteList = new List<string>();


        [TestInitialize]
        public void CreateRepository()
        {
            Mock<ILogger> logger = new Mock<ILogger>();
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
            var config = builder.Build();
            var _dbconfig = config.GetSection("ReportingDBSettings").Get<CosmoDBSettings>();

            _repository = new ReportingStorageRepository(new CosmoDBStorageInitializer(_dbconfig), logger.Object);
        }

        [TestCleanup]
        public async Task CleanUp()
        {
            RequestOptions requestOptions = new RequestOptions
            {
                PartitionKey = new PartitionKey(_tenantId)
            };
            foreach (string documentId in _idToDeleteList)
            {
                try
                {
                    await _repository.DeleteDocumentAsync(documentId, requestOptions);
                }
                catch (Microsoft.Azure.Documents.DocumentClientException)
                {
                    continue;
                }
            }

            _idToDeleteList.Clear();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullInitializerThrowsArgumentNullException()
        {
            Mock<ILogger> logger = new Mock<ILogger>();
            var sut = new RawEventStorageRepository(null, logger.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task ReportingStorageRepository_GetIntervals_Null_User_Returns_NullReferenceException()
        {
            //Arrange: create EffectiveAuthorization without user
            var permission = new HAS.Core.Domain.Permission() { Application = "NonExisting", Id = "NonExisting", Description = "NonExisting" };
            var target = new ExternalId() { Id = "NonExisting", Context = "NonExisting" };
            var effectiveAuthorization = new EffectiveAuthorization()
            {
                TenantId = "NonExisting" + DateTime.Now.Ticks,
                Permission = permission,
                User = null,
                Target = target
            };

            //Act: Attempt to get EffectiveAuthorization intervals
            var intervals = await _repository.GetIntervals(effectiveAuthorization);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task ReportingStorageRepository_GetIntervals_Null_Permission_Returns_NullReferenceException()
        {
            //Arrange: create EffectiveAuthorization without user
            var owner = new ExternalId() { Context = "NonExisting", Id = "NonExisting" };
            var target = new ExternalId() { Id = "NonExisting", Context = "NonExisting" };
            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = "NonExisting" + DateTime.Now.Ticks, Permission = null, User = owner, Target = target };

            //Act: Attempt to get EffectiveAuthorization intervals
            var intervals = await _repository.GetIntervals(effectiveAuthorization);
        }

        [TestMethod]
        public async Task ReportingStorageRepository_GetIntervals_NonExistingEffectiveAuthorization_Returns_EmptyList()
        {
            //Arrange: create Non-existing EffectiveAuthorization
            var owner = new ExternalId() { Context = "NonExisting", Id = "NonExisting" };
            var permission = new HAS.Core.Domain.Permission() { Application = "NonExisting", Id = "NonExisting", Description = "NonExisting" };
            var target = new ExternalId() { Id = "NonExisting", Context = "NonExisting" };
            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = "NonExisting" + DateTime.Now.Ticks, Permission = permission, User = owner, Target = target };

            //Act: Attempt to get EffectiveAuthorization intervals
            var intervals = await _repository.GetIntervals(effectiveAuthorization);

            //Assert: check that there is no returned data
            Assert.IsNotNull(intervals);
            Assert.AreEqual(0, intervals.Count);
        }

        [TestMethod]
        public async Task ReportingStorageRepository_SaveAsync_OnSuccessfulWritingEffectiveAuthorizationInterval_Without_TargetPerson_Returns_EffectiveAuthorizationInterval_Id()
        {
            //Arrange: create EffectiveAuthorizationInterval data to be stored
            var owner = new ExternalId() { Context = _ownerContext, Id = _ownerId };
            var user = new Person(owner, new PersonalInfo() { BirthDate = _birthDate1, Initials = _initials1, LastNameAtBirth = _lastNameAtBirth1, LastNameAtBirthPrefix = _lastNameAtBirthPrefix1 });
            var permission = new HAS.Core.Domain.Permission() { Application = _applicationName, Id = _Identifier, Description = _Identifier };
            var interval = new Interval(_dateTimeStart1, null);
            var effectiveAuthorizationInterval = new EffectiveAuthorizationInterval(interval, user, null, permission, _tenantId);
            var intervalList = new List<EffectiveAuthorizationInterval>();
            intervalList.Add(effectiveAuthorizationInterval);

            //Act: Attempt to write effectiveAuthorizationInterval to repository
            _idToDeleteList.AddRange((await _repository.SaveAsync(intervalList)).Split(","));

            //Assert: check that there is a returned Id with some data
            Assert.IsNotNull(_idToDeleteList[0]);
            Assert.IsTrue(_idToDeleteList[0].Length > 0);
        }

        [TestMethod]
        public async Task ReportingStorageRepository_SaveAsync_OnSuccessfulWritingEffectiveAuthorizationInterval_With_TargetPerson_Returns_EffectiveAuthorizationInterval_Id()
        {
            //Arrange: create EffectiveAuthorizationInterval data to be stored
            var owner = new ExternalId() { Context = _ownerContext, Id = _ownerId };
            var user = new Person(owner, new PersonalInfo() { BirthDate = _birthDate1, Initials = _initials1, LastNameAtBirth = _lastNameAtBirth1, LastNameAtBirthPrefix = _lastNameAtBirthPrefix1 });

            var target = new ExternalId() { Context = _ownerContext, Id = _targetId };
            var targetPerson = new Person(target, new PersonalInfo() { BirthDate = _birthDate2, Initials = _initials2, LastNameAtBirth = _lastNameAtBirth2, LastNameAtBirthPrefix = _lastNameAtBirthPrefix2 });

            var permission = new HAS.Core.Domain.Permission() { Application = _applicationName, Id = _Identifier, Description = _Identifier };
            var interval = new Interval(_dateTimeStart1, null);
            var effectiveAuthorizationInterval = new EffectiveAuthorizationInterval(interval, user, targetPerson, permission, _tenantId);
            var intervalList = new List<EffectiveAuthorizationInterval>();
            intervalList.Add(effectiveAuthorizationInterval);

            //Act: Attempt to write effectiveAuthorizationInterval to repository
            _idToDeleteList.AddRange((await _repository.SaveAsync(intervalList)).Split(","));

            //Assert: check that there is a returned Id with some data
            Assert.IsNotNull(_idToDeleteList[0]);
            Assert.IsTrue(_idToDeleteList[0].Length > 0);
        }

        [TestMethod]
        public async Task ReportingStorageRepository_GetIntervals_ExistingEffectiveAuthorizationInterval_Without_TargetPerson_Returns_EffectiveAuthorizationGrantedEvent_With_Right_EffectiveAuthorization()
        {
            //Arrange: create EffectiveAuthorizationInterval data to be stored
            var owner = new ExternalId() { Context = _ownerContext, Id = _ownerId };
            var user = new Person(owner, new PersonalInfo() { BirthDate = _birthDate1, Initials = _initials1, LastNameAtBirth = _lastNameAtBirth1, LastNameAtBirthPrefix = _lastNameAtBirthPrefix1 });
            var permission = new HAS.Core.Domain.Permission() { Application = _applicationName, Id = _Identifier, Description = _Identifier };
            var interval = new Interval(_dateTimeStart1, null);
            var effectiveAuthorizationInterval = new EffectiveAuthorizationInterval(interval, user, null, permission, _tenantId);

            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = _tenantId, Permission = permission, User = owner };

            var intervalList = new List<EffectiveAuthorizationInterval>();
            intervalList.Add(effectiveAuthorizationInterval);

            //Act: Attempt to write effectiveAuthorizationInterval to repository and getting it back
            _idToDeleteList.AddRange((await _repository.SaveAsync(intervalList)).Split(","));

            var effectiveAuthorizationIntervals = await _repository.GetIntervals(effectiveAuthorization);

            //Assert: check that there is a returned Id with some data
            Assert.IsNotNull(effectiveAuthorizationIntervals);
            Assert.IsTrue(effectiveAuthorizationIntervals.Count > 0);
            Assert.IsInstanceOfType(effectiveAuthorizationIntervals[0], typeof(EffectiveAuthorizationInterval));
            Assert.AreEqual(effectiveAuthorizationIntervals[0].TenantId, effectiveAuthorization.TenantId);
            Assert.AreEqual(effectiveAuthorizationIntervals[0].User.Key, effectiveAuthorization.User);
            Assert.AreEqual(effectiveAuthorizationIntervals[0].Permission, effectiveAuthorization.Permission);
        }

        [TestMethod]
        public async Task ReportingStorageRepository_GetIntervals_ExistingEffectiveAuthorizationInterval_With_TargetPerson_Returns_EffectiveAuthorizationGrantedEvent_With_Right_EffectiveAuthorization()
        {
            //Arrange: create EffectiveAuthorizationInterval data to be stored
            var owner = new ExternalId() { Context = _ownerContext, Id = _ownerId };
            var user = new Person(owner, new PersonalInfo()
            {
                BirthDate = _birthDate1,
                Initials = _initials1,
                LastNameAtBirth = _lastNameAtBirth1,
                LastNameAtBirthPrefix = _lastNameAtBirthPrefix1
            });

            var target = new ExternalId() { Context = _ownerContext, Id = _targetId };
            var targetPerson = new Person(target,
                new PersonalInfo()
                {
                    BirthDate = _birthDate2,
                    Initials = _initials2,
                    LastNameAtBirth = _lastNameAtBirth2,
                    LastNameAtBirthPrefix = _lastNameAtBirthPrefix2
                });

            var permission = new HAS.Core.Domain.Permission() {
                Application = _applicationName,
                Id = _Identifier,
                Description = _Identifier
            };
            var interval = new Interval(_dateTimeStart1, null);
            var effectiveAuthorizationInterval = new EffectiveAuthorizationInterval(interval, user, targetPerson, permission, _tenantId);

            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = _tenantId, Permission = permission, User = owner, Target = target };

            var intervalList = new List<EffectiveAuthorizationInterval>();
            intervalList.Add(effectiveAuthorizationInterval);

            //Act: Attempt to write effectiveAuthorizationInterval to repository and getting it back
            _idToDeleteList.AddRange((await _repository.SaveAsync(intervalList)).Split(","));

            var effectiveAuthorizationIntervals = await _repository.GetIntervals(effectiveAuthorization);

            //Assert: check that there is a returned Id with some data
            Assert.IsNotNull(effectiveAuthorizationIntervals);
            Assert.IsTrue(effectiveAuthorizationIntervals.Count > 0);
            Assert.IsInstanceOfType(effectiveAuthorizationIntervals[0], typeof(EffectiveAuthorizationInterval));
            Assert.AreEqual(effectiveAuthorizationIntervals[0].TenantId, effectiveAuthorization.TenantId);
            Assert.AreEqual(effectiveAuthorizationIntervals[0].User.Key, effectiveAuthorization.User);
            Assert.AreEqual(effectiveAuthorizationIntervals[0].TargetPerson.Key, effectiveAuthorization.Target);
            Assert.AreEqual(effectiveAuthorizationIntervals[0].Permission, effectiveAuthorization.Permission);

        }

        [TestMethod]
        public async Task ReportingStorageRepository_DeleteIntervals_ExistingEffectiveAuthorizationIntervals_Deletes_All_With_Same_Effective_Authorization_Values()
        {
            //Arrange: create EffectiveAuthorizationInterval data to be stored
            var owner = new ExternalId() { Context = _ownerContext, Id = _ownerId };
            var user = new Person(owner, new PersonalInfo() { BirthDate = _birthDate1, Initials = _initials1, LastNameAtBirth = _lastNameAtBirth1, LastNameAtBirthPrefix = _lastNameAtBirthPrefix1 });

            var target = new ExternalId() { Context = _ownerContext, Id = _targetId };
            var targetPerson = new Person(target, new PersonalInfo() { BirthDate = _birthDate2, Initials = _initials2, LastNameAtBirth = _lastNameAtBirth2, LastNameAtBirthPrefix = _lastNameAtBirthPrefix2 });

            var permission = new HAS.Core.Domain.Permission() { Application = _applicationName, Id = _Identifier, Description = _Identifier };
            var interval = new Interval(_dateTimeStart1, null);
            var effectiveAuthorizationInterval = new EffectiveAuthorizationInterval(interval, user, targetPerson, permission, _tenantId);

            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = _tenantId, Permission = permission, User = owner, Target = target };

            var intervalList = new List<EffectiveAuthorizationInterval>();
            intervalList.Add(effectiveAuthorizationInterval);

            //Act: Attempt to write effectiveAuthorizationInterval to repository, delete it and try to retrieve it back
            _idToDeleteList.AddRange((await _repository.SaveAsync(intervalList)).Split(",")); //In case test fails

            await _repository.DeleteIntervals(effectiveAuthorization);

            var effectiveAuthorizationIntervals = await _repository.GetIntervals(effectiveAuthorization);

            //Assert: check that there is no intervals
            Assert.IsNotNull(effectiveAuthorizationIntervals);
            Assert.IsTrue(effectiveAuthorizationIntervals.Count == 0);
        }

        [TestMethod]
        public async Task ReportingStorageRepository_DeleteIntervals_ExistingEffectiveAuthorizationIntervals_doesnt_delete_unrelated_Intervals()
        {
            int unrelatedEffectiveAuthorizationIntervals = 10;
            //Arrange: create EffectiveAuthorizationInterval data to be stored
            var owner = new ExternalId() { Context = _ownerContext, Id = _ownerId };
            var user = new Person(owner, new PersonalInfo() { BirthDate = _birthDate1, Initials = _initials1, LastNameAtBirth = _lastNameAtBirth1, LastNameAtBirthPrefix = _lastNameAtBirthPrefix1 });

            var target = new ExternalId() { Context = _ownerContext, Id = _targetId };
            var targetPerson = new Person(target, new PersonalInfo() { BirthDate = _birthDate2, Initials = _initials2, LastNameAtBirth = _lastNameAtBirth2, LastNameAtBirthPrefix = _lastNameAtBirthPrefix2 });

            var permission = new HAS.Core.Domain.Permission() { Application = _applicationName, Id = _Identifier, Description = _Identifier };
            var interval = new Interval(_dateTimeStart1, null);
            var effectiveAuthorizationInterval = new EffectiveAuthorizationInterval(interval, user, targetPerson, permission, _tenantId);

            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = _tenantId, Permission = permission, User = owner, Target = target };

            var intervalList = new List<EffectiveAuthorizationInterval>();
            intervalList.Add(effectiveAuthorizationInterval);

            EffectiveAuthorizationInterval auxEffectiveAuthInt;
            for (int i = 0; i < unrelatedEffectiveAuthorizationIntervals; i++)
            {
                //Just change the tenantId and the EffectiveAuthorization is different
                auxEffectiveAuthInt = new EffectiveAuthorizationInterval(interval, user, targetPerson, permission, _tenantId + i);
                var deleteInValidRecordIfExists = new EffectiveAuthorization() { TenantId = _tenantId + i, Permission = permission, User = owner, Target = target };
                await _repository.DeleteIntervals(deleteInValidRecordIfExists);
                intervalList.Add(auxEffectiveAuthInt);
            }

            //Act: Attempt to write effectiveAuthorizationInterval to repository, delete it and try to retrieve it back
            _idToDeleteList.AddRange((await _repository.SaveAsync(intervalList)).Split(",")); //In case test fails

            await _repository.DeleteIntervals(effectiveAuthorization);

            var intervals = new List<EffectiveAuthorizationInterval>();
            for (int i = 0; i < unrelatedEffectiveAuthorizationIntervals; i++)
            {
                intervals.AddRange(await _repository.GetIntervals(new EffectiveAuthorization() { User = owner, Permission = permission, Target = target, TenantId = _tenantId + i }));
            }

            var unwantedIntervals = await _repository.GetIntervals(effectiveAuthorization);

            //Assert: check that there is no intervals
            Assert.IsNotNull(intervals);
            Assert.AreEqual(unrelatedEffectiveAuthorizationIntervals, intervals.Count);
            Assert.IsNotNull(unwantedIntervals);
            Assert.AreEqual(0, unwantedIntervals.Count);
        }

        [TestMethod]
        public async Task ReportingStorageRepository_SaveAsync_ExistingInterval_Updates_And_Doesnt_Add_ANew()
        {
            //Arrange: create and insert interval to be udpated
            var owner = new ExternalId() { Context = _ownerContext, Id = _ownerId };
            var user = new Person(owner, new PersonalInfo() { BirthDate = _birthDate1, Initials = _initials1, LastNameAtBirth = _lastNameAtBirth1, LastNameAtBirthPrefix = _lastNameAtBirthPrefix1 });

            var target = new ExternalId() { Context = _ownerContext, Id = _targetId };
            var targetPerson = new Person(target, new PersonalInfo() { BirthDate = _birthDate2, Initials = _initials2, LastNameAtBirth = _lastNameAtBirth2, LastNameAtBirthPrefix = _lastNameAtBirthPrefix2 });

            var permission = new HAS.Core.Domain.Permission() { Application = _applicationName, Id = _Identifier, Description = _Identifier };
            var interval = new Interval(_dateTimeStart1, null);
            var effectiveAuthorizationInterval = new EffectiveAuthorizationInterval(interval, user, targetPerson, permission, _tenantId);

            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = _tenantId, Permission = permission, User = owner, Target = target };

            var intervalList = new List<EffectiveAuthorizationInterval>();
            intervalList.Add(effectiveAuthorizationInterval);

            _idToDeleteList.AddRange((await _repository.SaveAsync(intervalList)).Split(",")); //In case test fails

            //Act: update the interval and save
            var updatedInterval = new Interval(interval.Start, _dateTimeEnd1);
            var updatedEffectiveAuthorizationInterval = new EffectiveAuthorizationInterval(updatedInterval, user, targetPerson, permission, _tenantId);

            var updatedInternalList = new List<EffectiveAuthorizationInterval>();
            updatedInternalList.Add(updatedEffectiveAuthorizationInterval);

            _idToDeleteList.AddRange((await _repository.SaveAsync(updatedInternalList)).Split(",")); //In case test fails

            //Assert: check that there is still only one interval
            var wantedIntervals = await _repository.GetIntervals(effectiveAuthorization);

            Assert.IsNotNull(wantedIntervals);
            Assert.AreEqual(1, wantedIntervals.Count);
            Assert.AreEqual(_tenantId, wantedIntervals[0].TenantId);
            Assert.AreEqual(owner, wantedIntervals[0].User.Key);
            Assert.AreEqual(permission, wantedIntervals[0].Permission);
            Assert.AreEqual(target, wantedIntervals[0].TargetPerson.Key);
            Assert.AreEqual(_dateTimeStart1, wantedIntervals[0].EffectiveInterval.Start);
            Assert.AreEqual(_dateTimeEnd1, wantedIntervals[0].EffectiveInterval.End);
        }

        [TestMethod]
        public async Task ReportingStorageRepository_SaveAsync_IntervalList_Only_Saved_Ones_Remain_Stored()
        {
            //Arrange: create and insert interval list
            var owner = new ExternalId() { Context = _ownerContext, Id = _ownerId };
            var user = new Person(owner, new PersonalInfo() { BirthDate = _birthDate1, Initials = _initials1, LastNameAtBirth = _lastNameAtBirth1, LastNameAtBirthPrefix = _lastNameAtBirthPrefix1 });

            var target = new ExternalId() { Context = _ownerContext, Id = _targetId };
            var targetPerson = new Person(target, new PersonalInfo() { BirthDate = _birthDate2, Initials = _initials2, LastNameAtBirth = _lastNameAtBirth2, LastNameAtBirthPrefix = _lastNameAtBirthPrefix2 });

            var permission = new HAS.Core.Domain.Permission() { Application = _applicationName, Id = _Identifier, Description = _Identifier };
            var intervalList = new List<EffectiveAuthorizationInterval>();

            EffectiveAuthorizationInterval auxInterval;

            auxInterval = new EffectiveAuthorizationInterval(new Interval(_dateTimeStart1, _dateTimeEnd1), user, targetPerson, permission, _tenantId);
            intervalList.Add(auxInterval);
            auxInterval = new EffectiveAuthorizationInterval(new Interval(_dateTimeStart1.AddDays(1), _dateTimeEnd1.AddDays(1)), user, targetPerson, permission, _tenantId);
            intervalList.Add(auxInterval);
            auxInterval = new EffectiveAuthorizationInterval(new Interval(_dateTimeStart1.AddDays(2), _dateTimeEnd1.AddDays(2)), user, targetPerson, permission, _tenantId);
            intervalList.Add(auxInterval);

            _idToDeleteList.AddRange((await _repository.SaveAsync(intervalList)).Split(",")); //In case test fails

            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = _tenantId, Permission = permission, User = owner, Target = target };

            //assert: check that the retrieved intervals from BBDD match
            var retrievedIntervals = (await _repository.GetIntervals(effectiveAuthorization)).OrderBy(x => x.EffectiveInterval.Start).ToList<EffectiveAuthorizationInterval>();

            Assert.IsNotNull(retrievedIntervals);
            Assert.AreEqual(3, retrievedIntervals.Count);

            Assert.AreEqual(_tenantId, retrievedIntervals[0].TenantId);
            Assert.AreEqual(owner, retrievedIntervals[0].User.Key);
            Assert.AreEqual(permission, retrievedIntervals[0].Permission);
            Assert.AreEqual(target, retrievedIntervals[0].TargetPerson.Key);
            Assert.AreEqual(_dateTimeStart1, retrievedIntervals[0].EffectiveInterval.Start);
            Assert.AreEqual(_dateTimeEnd1, retrievedIntervals[0].EffectiveInterval.End);

            Assert.AreEqual(_tenantId, retrievedIntervals[1].TenantId);
            Assert.AreEqual(owner, retrievedIntervals[1].User.Key);
            Assert.AreEqual(permission, retrievedIntervals[1].Permission);
            Assert.AreEqual(target, retrievedIntervals[1].TargetPerson.Key);
            Assert.AreEqual(_dateTimeStart1.AddDays(1), retrievedIntervals[1].EffectiveInterval.Start);
            Assert.AreEqual(_dateTimeEnd1.AddDays(1), retrievedIntervals[1].EffectiveInterval.End);

            Assert.AreEqual(_tenantId, retrievedIntervals[2].TenantId);
            Assert.AreEqual(owner, retrievedIntervals[2].User.Key);
            Assert.AreEqual(permission, retrievedIntervals[2].Permission);
            Assert.AreEqual(target, retrievedIntervals[2].TargetPerson.Key);
            Assert.AreEqual(_dateTimeStart1.AddDays(2), retrievedIntervals[2].EffectiveInterval.Start);
            Assert.AreEqual(_dateTimeEnd1.AddDays(2), retrievedIntervals[2].EffectiveInterval.End);
        }
    }
}
