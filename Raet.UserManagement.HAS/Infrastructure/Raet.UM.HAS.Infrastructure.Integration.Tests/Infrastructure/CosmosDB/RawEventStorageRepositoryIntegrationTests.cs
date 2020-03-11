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
    public class RawEventStorageRepositoryRepositoryIntegrationTests
    {
        RawEventStorageRepository _repository;

        readonly string _tenantId = "RawEventStorageRepositoryRepositoryIntegrationTests";

        readonly string _ownerId = "ID10000";
        readonly string _ownerContext = "Youforce.Users";

        readonly string _applicationName = "YOUFORCE";
        readonly string _Identifier = "HomeAccess";


        readonly DateTime _from = new DateTime(2018, 1, 1).ToUniversalTime();
        readonly DateTime _until = new DateTime(2018, 5, 1).ToUniversalTime();

        readonly List<string> _eventIdList = new List<string>();

        [TestInitialize]
        public void CreateRepository()
        {
            Mock<ILogger> logger = new Mock<ILogger>();
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
            var config = builder.Build();
            var _dbconfig = config.GetSection("CosmoDBSettings").Get<CosmoDBSettings>();

            _repository = new RawEventStorageRepository(new CosmoDBStorageInitializer(_dbconfig), logger.Object);
        }

        [TestCleanup]
        public async Task CleanUp()
        {
            foreach (string eventId in _eventIdList)
            {
                await _repository.DeleteDocumentAsync(eventId, new RequestOptions() { PartitionKey = new PartitionKey(Undefined.Value) });
            }

            _eventIdList.Clear();
        }

        /// <summary>
        /// This test is used to try REAL storage initialization on integration step
        /// So keep it ignored til needed
        /// </summary>
        [TestMethod]
        [Ignore]
        public void RawEventStorageRepositoryIsProperlyInitialized()
        {
            Assert.IsNotNull(_repository);
            Assert.IsInstanceOfType(_repository, typeof(RawEventStorageRepository));
         }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RawEventStorageRepositoryWithNullInitializerThrowsArgumentNullException()
        {
            Mock<ILogger> logger = new Mock<ILogger>();
            var sut = new RawEventStorageRepository(null, logger.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task RawEventStorageRepository_GetRawEventsAsync_Null_User_Returns_NullReferenceException()
        {
            //Arrange: create EffectiveAuthorization without user
            var permission = new HAS.Core.Domain.Permission() { Application = "NonExisting", Id = "NonExisting", Description = "NonExisting" };
            var target = new ExternalId() { Id = "NonExisting", Context = "NonExisting" };
            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = "NonExisting" + DateTime.Now.Ticks, Permission = permission, User = null, Target = target };

            //Act: Attempt to get related events
            var rawEvent = await _repository.GetRawEventsAsync(effectiveAuthorization);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task RawEventStorageRepository_GetRawEventsAsync_Null_Permission_Returns_NullReferenceException()
        {
            //Arrange: create EffectiveAuthorization without user
            var owner = new ExternalId() { Context = "NonExisting", Id = "NonExisting" };
            var target = new ExternalId() { Id = "NonExisting", Context = "NonExisting" };
            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = "NonExisting" + DateTime.Now.Ticks, Permission = null, User = owner, Target = target };

            //Act: Attempt to get related events
            var rawEvent = await _repository.GetRawEventsAsync(effectiveAuthorization);
        }

        [TestMethod]
        public async Task RawEventStorageRepository_GetRawEventsAsync_NonExistingEffectiveAuthorization_Returns_EmptyList()
        {
            //Arrange: create Non-existing EffectiveAuthorization
            var owner = new ExternalId() { Context = "NonExisting", Id = "NonExisting" };
            var permission = new HAS.Core.Domain.Permission() { Application = "NonExisting", Id = "NonExisting", Description = "NonExisting" };
            var target = new ExternalId() { Id = "NonExisting", Context = "NonExisting" };
            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = "NonExisting" + DateTime.Now.Ticks, Permission = permission, User = owner, Target = target };

            //Act: Attempt to get nonexisting event
            var rawEvent = await _repository.GetRawEventsAsync(effectiveAuthorization);

            //Assert: check that there is no returned data
            Assert.IsNotNull(rawEvent);
            Assert.AreEqual(0, rawEvent.Count);
        }

        [TestMethod]
        public async Task RawEventStorageRepository_WriteRawEventAsync_OnSuccessfulWritingGrantedEvent_Returns_Event_Id()
        {
            //Arrange: create event data to be stored
            var owner = new ExternalId() { Context = _ownerContext, Id = _ownerId };
            var permission = new HAS.Core.Domain.Permission() { Application = _applicationName, Id = _Identifier, Description = _Identifier };
            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = _tenantId, Permission = permission, User = owner};
            var eaEvent = new EffectiveAuthorizationGrantedEvent() { From = _from, EffectiveAuthorization = effectiveAuthorization, DateCreated = DateTime.Now };

            //Act: Attempt to write event to repository
            _eventIdList.Add(await _repository.WriteRawEventAsync(eaEvent));

            //Assert: check that there is a returned Id with some data
            Assert.IsNotNull(_eventIdList[0]);
            Assert.IsTrue(_eventIdList[0].Length > 0);
        }

        [TestMethod]
        public async Task RawEventStorageRepository_WriteRawEventAsync_OnSuccessfulWritingRevokedEvent_Returns_Event_Id()
        {
            //Arrange: create event data to be stored
            var owner = new ExternalId() { Context = _ownerContext, Id = _ownerId };
            var permission = new HAS.Core.Domain.Permission() { Application = _applicationName, Id = _Identifier, Description = _Identifier };
            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = _tenantId, Permission = permission, User = owner };
            var eaEvent = new EffectiveAuthorizationRevokedEvent() { Until = _until, EffectiveAuthorization = effectiveAuthorization, DateCreated = DateTime.Now };

            //Act: Attempt to write event to repository
            _eventIdList.Add(await _repository.WriteRawEventAsync(eaEvent));

            //Assert: check that there is a returned Id with some data
            Assert.IsNotNull(_eventIdList[0]);
            Assert.IsTrue(_eventIdList[0].Length > 0);
        }

        [TestMethod]
        public async Task RawEventStorageRepository_GetRawEventsAsync_ExistingGrantedEvent_Returns_EffectiveAuthorizationGrantedEvent_With_Right_EffectiveAuthorization()
        {
            //Arrange: create event data to be stored
            var owner = new ExternalId() { Context = _ownerContext, Id = _ownerId };
            var permission = new HAS.Core.Domain.Permission() { Application = _applicationName, Id = _Identifier, Description = _Identifier };
            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = _tenantId, Permission = permission, User = owner };
            var eaEvent = new EffectiveAuthorizationGrantedEvent() { From = _from, EffectiveAuthorization = effectiveAuthorization, DateCreated = DateTime.Now };

            //Act: Attempt to write event to repository
            _eventIdList.Add(await _repository.WriteRawEventAsync(eaEvent));

            //Act: Attempt to get event
            var rawEventList = await _repository.GetRawEventsAsync(effectiveAuthorization);

            //Assert: check that there is a returned Id with some data
            Assert.IsNotNull(rawEventList);
            Assert.IsTrue(rawEventList.Count > 0);
            Assert.IsInstanceOfType(rawEventList[0], typeof(EffectiveAuthorizationGrantedEvent));
            Assert.AreEqual(rawEventList[0].EffectiveAuthorization, effectiveAuthorization);
        }

        [TestMethod]
        public async Task RawEventStorageRepository_GetRawEventsAsync_ExistingRevokedEvent_Returns_EffectiveAuthorizationRevokedEvent_With_Right_EffectiveAuthorization()
        {
            //Arrange: create event data to be stored
            var owner = new ExternalId() { Context = _ownerContext, Id = _ownerId };
            var permission = new HAS.Core.Domain.Permission() { Application = _applicationName, Id = _Identifier, Description = _Identifier };
            var effectiveAuthorization = new EffectiveAuthorization() { TenantId = _tenantId, Permission = permission, User = owner };
            var eaEvent = new EffectiveAuthorizationRevokedEvent() { Until = _until, EffectiveAuthorization = effectiveAuthorization, DateCreated = DateTime.Now };

            //Act: Attempt to write event to repository
            _eventIdList.Add(await _repository.WriteRawEventAsync(eaEvent));

            //Act: Attempt to get nonexisting event
            var rawEventList = await _repository.GetRawEventsAsync(effectiveAuthorization);

            var revokedRawEventList = rawEventList.Where(e=>e.GetType() == typeof(EffectiveAuthorizationRevokedEvent));

            //Assert: check that there is a returned Id with some data
            Assert.IsNotNull(rawEventList);
            Assert.IsTrue(rawEventList.Count > 0);
            Assert.IsNotNull(revokedRawEventList);
            Assert.AreEqual(rawEventList[0].EffectiveAuthorization, effectiveAuthorization);
        }
    }
}
