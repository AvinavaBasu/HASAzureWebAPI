using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raet.UM.HAS.Client;
using Raet.UM.HAS.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Raet.UM.HAS.Client.Interfaces;

namespace Raet.UM.HAS.Client.Tests
{
    [TestClass]
    public class PushClientTests
    {
        private const string _defaultTenantId = "TenantId1";

        [TestMethod]
        public void PushEffectiveAuthorizationGrantedAsync_EffectiveAuthorization_Null_Throws_Argument_Null_Exception()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => pushClient.PushEffectiveAuthorizationGrantedAsync(null, new DateTime(2018, 1, 1)));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationRevokedAsync_EffectiveAuthorization_Null_Throws_ArgumentNullException()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            Assert.ThrowsExceptionAsync<ArgumentNullException>(() => pushClient.PushEffectiveAuthorizationRevokedAsync(null, new DateTime(2018, 1, 1)));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationGrantedAsync_EffectiveAuthorization_Has_Null_TenantId_Throws_ValidationException_TenantId_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var effectiveAuthorization = new EffectiveAuthorization(null, null, null, null);
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationGrantedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("TenantId field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationRevokedAsync_EffectiveAuthorization_Has_Null_TenantId_Throws_ValidationException_TenantId_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var effectiveAuthorization = new EffectiveAuthorization(null, null, null, null);
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationRevokedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("TenantId field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationGrantedAsync_EffectiveAuthorization_Has_No_User_Returns_ValidationException_User_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var permission = new Permission("Identifier1", "Application1", "Description");
            var target = new ExternalId("Context1", "ID1");
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, null, permission, target);
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationGrantedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("User field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationRevokedAsync_EffectiveAuthorization_Has_No_User_Returns_ValidationException_User_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var permission = new Permission("Identifier1", "Application1", "Description");
            var target = new ExternalId("Context1", "ID1");
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, null, permission, target);
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationRevokedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("User field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationGrantedAsync_EffectiveAuthorization_Has_User_Without_Context_Returns_ValidationException_Context_Field_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId(null, null);
            var permission = new Permission("identifier1", "application1", "description1");
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, permission, null);
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationGrantedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Context field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationRevokedAsync_EffectiveAuthorization_Has_User_Without_Context_Returns_ValidationException_Context_Field_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId(null, null);
            var permission = new Permission("identifier1", "application1", "description1");
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, permission, null);
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationRevokedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Context field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationGrantedAsync_EffectiveAuthorization_Has_User_With_EmptyString_Context_Returns_ValidationException_Context_Field_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId(String.Empty, null);
            var permission = new Permission("identifier1", "application1", "description1");
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, permission, null);
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationGrantedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Context field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationRevokedAsync_EffectiveAuthorization_Has_User_With_EmptyString_Context_Returns_ValidationException_Context_Field_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId(String.Empty, null);
            var permission = new Permission("identifier1", "application1", "description1");
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, permission, null);
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationRevokedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Context field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationGrantedAsync_EffectiveAuthorization_Has_User_Without_Id_Returns_ValidationException_Id_Field_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId("Context01", null);
            var permission = new Permission("identifier1", "application1", "description1");
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, permission, null);
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationGrantedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Id field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationRevokedAsync_EffectiveAuthorization_Has_User_Without_Id_Returns_ValidationException_Id_Field_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId("Context01", null);
            var permission = new Permission("identifier1", "application1", "description1");
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, permission, null);
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationRevokedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Id field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationGrantedAsync_EffectiveAuthorization_Has_User_With_EmptyString_Id_Returns_ValidationException_Id_Field_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId("Context01", String.Empty);
            var permission = new Permission("identifier1", "application1", "description1");
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, permission, null);
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationGrantedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Id field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationRevokedAsync_EffectiveAuthorization_Has_User_With_EmptyString_Id_Returns_ValidationException_Id_Field_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId("Context01", String.Empty);
            var permission = new Permission("identifier1", "application1", "description1");
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, permission, null);
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationRevokedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Id field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationGrantedAsync_EffectiveAuthorization_Has_No_Permission_Returns_ValidationException_Permission_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId("Context1", "ID1");
            var target = new ExternalId("Context2", "ID2");
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, null, target);
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationGrantedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Permission field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationRevokedAsync_EffectiveAuthorization_Has_No_Permission_Returns_ValidationException_Permission_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId("Context1", "ID1");
            var target = new ExternalId("Context2", "ID2");
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, null, target);
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationRevokedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Permission field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationGrantedAsync_EffectiveAuthorization_Permission_Has_No_Identifier_Returns_ValidationException_Identifier_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId("Context1", "ID1");
            var permission = new Permission(null, "Application1", "Description1");
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, permission, null);
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationGrantedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Id field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationRevokedAsync_EffectiveAuthorization_Permission_Has_No_Identifier_Returns_ValidationException_Identifier_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId("Context1", "ID1");
            var permission = new Permission(null, "Application1", "Description1");
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, permission, null);
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationRevokedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Id field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationGrantedAsync_EffectiveAuthorization_Permission_Has_EmptyString_Identifier_Returns_ValidationException_Identifier_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId("Context1", "ID1");
            var permission = new Permission(String.Empty, "Application1", "Description1");
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, permission, null);
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationGrantedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Id field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationRevokedAsync_EffectiveAuthorization_Permission_Has_EmptyString_Identifier_Returns_ValidationException_Identifier_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId("Context1", "ID1");
            var permission = new Permission(String.Empty, "Application1", "Description1");
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, permission, null);
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationRevokedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Id field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationGrantedAsync_EffectiveAuthorization_Permission_Has_No_Application_Returns_ValidationException_Application_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId("Context1", "ID1");
            var permission = new Permission("Identifier01", null, "Description1");
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, permission, null);
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationGrantedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Application field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationRevokedAsync_EffectiveAuthorization_Permission_Has_No_Application_Returns_ValidationException_Application_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId("Context1", "ID1");
            var permission = new Permission("Identifier01", null, "Description1");
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, permission, null);
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationRevokedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Application field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationGrantedAsync_EffectiveAuthorization_Permission_Has_EmptyString_Application_Returns_ValidationException_Application_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId("Context1", "ID1");
            var permission = new Permission("Identifier01", string.Empty, "Description1");
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, permission, null);
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationGrantedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Application field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationRevokedAsync_EffectiveAuthorization_Permission_Has_EmptyString_Application_Returns_ValidationException_Application_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId("Context1", "ID1");
            var permission = new Permission("Identifier01", string.Empty, "Description1");
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, permission, null);
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationRevokedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Application field is required"));
        }

        [TestMethod]
        public async Task PushEffectiveAuthorizationGrantedAsync_EffectiveAuthorization_Without_TargetPerson_Calls_IEventPusher_PushGrantedAsync()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var source = new ExternalId("Context1", "ID1");
            var permission = new Permission("Identifier1", "Application1", "Description1");
            var authorization = new EffectiveAuthorization(_defaultTenantId, source, permission, null);
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act
            await pushClient.PushEffectiveAuthorizationGrantedAsync(authorization, new DateTime(2018, 1, 1));

            //Assert
            eventPusherMock.Verify(m => m.PushGrantedAsync(It.IsAny<EffectiveAuthorizationEvent>()), Times.Once);
        }

        [TestMethod]
        public async Task PushEffectiveAuthorizationRevokedAsync_EffectiveAuthorization_Without_TargetPerson_Calls_IEventPusher_PushRevokedAsync()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var source = new ExternalId("Context1", "ID1");
            var permission = new Permission("Identifier1", "Application1", "Description1");
            var authorization = new EffectiveAuthorization(_defaultTenantId, source, permission, null);
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act
            await pushClient.PushEffectiveAuthorizationRevokedAsync(authorization, new DateTime(2018, 1, 1));

            //Assert
            eventPusherMock.Verify(m => m.PushRevokedAsync(It.IsAny<EffectiveAuthorizationEvent>()), Times.Once);
        }

        [TestMethod]
        public void PushEffectiveAuthorizationGrantedAsync_EffectiveAuthorization_Has_TargetPerson_Without_Context_Returns_ValidationException_Context_Field_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId("Context1", "ID1");
            var permission = new Permission("identifier1", "application1", "description1");
            var target = new ExternalId(null, null);
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, permission, target);
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationGrantedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Context field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationRevokedAsync_EffectiveAuthorization_Has_TargetPerson_Without_Context_Returns_ValidationException_Context_Field_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId("Context1", "ID1");
            var permission = new Permission("identifier1", "application1", "description1");
            var target = new ExternalId(null, null);
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, permission, target);
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationRevokedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Context field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationGrantedAsync_EffectiveAuthorization_Has_TargetPerson_Without_Id_Returns_ValidationException_Id_Field_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId("Context1", "ID1");
            var permission = new Permission("identifier1", "application1", "description1");
            var target = new ExternalId("Context1", null);
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, permission, target);
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationGrantedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Id field is required"));
        }

        [TestMethod]
        public void PushEffectiveAuthorizationRevokedAsync_EffectiveAuthorization_Has_TargetPerson_Without_Id_Returns_ValidationException_Id_Field_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var subject = new ExternalId("Context1", "ID1");
            var permission = new Permission("identifier1", "application1", "description1");
            var target = new ExternalId("Context1", null);
            var effectiveAuthorization = new EffectiveAuthorization(_defaultTenantId, subject, permission, target);
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushEffectiveAuthorizationRevokedAsync(effectiveAuthorization, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Id field is required"));
        }

        [TestMethod]
        public async Task PushEffectiveAuthorizationGrantedAsync_EffectiveAuthorization_Full_IEventPusher_PushGrantedAsync()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var source = new ExternalId("Context1", "ID1");
            var permission = new Permission("Identifier1", "Application1", "Description1");
            var target = new ExternalId("Context1", "ID2");
            var authorization = new EffectiveAuthorization(_defaultTenantId, source, permission, target);
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act
            await pushClient.PushEffectiveAuthorizationGrantedAsync(authorization, new DateTime(2018, 1, 1));

            //Assert
            eventPusherMock.Verify(m => m.PushGrantedAsync(It.IsAny<EffectiveAuthorizationEvent>()), Times.Once);
        }

        [TestMethod]
        public async Task PushEffecitveAuthorizationRevokedAsync_EffectiveAuthorization_Full_Calls_IEventPusher_PushRevokedAsync()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var source = new ExternalId("Context1", "ID1");
            var permission = new Permission("Identifier1", "Application1", "Description1");
            var target = new ExternalId("Context1", "ID2");
            var authorization = new EffectiveAuthorization(_defaultTenantId, source, permission, target);
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act
            await pushClient.PushEffectiveAuthorizationRevokedAsync(authorization, new DateTime(2018, 1, 1));

            //Assert
            eventPusherMock.Verify(m => m.PushRevokedAsync(It.IsAny<EffectiveAuthorizationEvent>()), Times.Once);
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationGrantedAsync_Without_TenantId_Returns_ValidationException_Context_Field_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId("Context1", "ID1");
            var permission = new Permission("identifier1", "application1", "description1");
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationGrantedAsync(null, user, permission, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("TenantId field is required"));
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationRevokedAsync_Without_TenantId_Returns_ValidationException_Context_Field_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId("Context1", "ID1");
            var permission = new Permission("identifier1", "application1", "description1");
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationRevokedAsync(null, user, permission, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("TenantId field is required"));
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationGrantedAsync_Without_User_Returns_ValidationException_User_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var permission = new Permission("Identifier1", "Application1", "Description");
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationGrantedAsync(_defaultTenantId, null, permission, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("User field is required"));
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationRevokedAsync_Without_User_Returns_ValidationException_User_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var permission = new Permission("Identifier1", "Application1", "Description");
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationRevokedAsync(_defaultTenantId, null, permission, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("User field is required"));
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationGrantedAsync_User_Without_Context_Returns_ValidationException_Context_Field_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId(null, null);
            var permission = new Permission("identifier1", "application1", "description1");
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationGrantedAsync(_defaultTenantId, user, permission, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Context field is required"));
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationRevokedAsync_User_Without_Context_Returns_ValidationException_Context_Field_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId(null, null);
            var permission = new Permission("identifier1", "application1", "description1");
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationRevokedAsync(_defaultTenantId, user, permission, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Context field is required"));
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationGrantedAsync_User_With_EmptyString_Context_Returns_ValidationException_Context_Field_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId(String.Empty, null);
            var permission = new Permission("identifier1", "application1", "description1");
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationGrantedAsync(_defaultTenantId, user, permission, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Context field is required"));
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationRevokedAsync_User_With_EmptyString_Context_Returns_ValidationException_Context_Field_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId(String.Empty, null);
            var permission = new Permission("identifier1", "application1", "description1");
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationRevokedAsync(_defaultTenantId, user, permission, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Context field is required"));
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationGrantedAsync_User_With_Null_Id_Returns_ValidationException_Id_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId("Context01", null);
            var permission = new Permission("identifier1", "application1", "description1");
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationGrantedAsync(_defaultTenantId, user, permission, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Id field is required"));
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationRevokedAsync_User_With_Null_Id_Returns_ValidationException_Id_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId("Context01", null);
            var permission = new Permission("identifier1", "application1", "description1");
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationRevokedAsync(_defaultTenantId, user, permission, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Id field is required"));
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationGrantedAsync_User_With_EmptyString_Id_Returns_ValidationException_Id_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId("Context01", String.Empty);
            var permission = new Permission("identifier1", "application1", "description1");
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationGrantedAsync(_defaultTenantId, user, permission, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Id field is required"));
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationRevokedAsync_User_With_EmptyString_Id_Returns_ValidationException_Id_Required()
        {
            //arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId("Context01", String.Empty);
            var permission = new Permission("identifier1", "application1", "description1");
            var pushClient = new PushClient(eventPusherMock.Object);

            //act & assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationRevokedAsync(_defaultTenantId, user, permission, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Id field is required"));
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationGrantedAsync_With_No_Permission_Returns_ValidationException_Permission_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId("Context1", "ID1");
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationGrantedAsync(_defaultTenantId, user, null, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Permission field is required"));
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationRevokedAsync_With_No_Permission_Returns_ValidationException_Permission_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId("Context1", "ID1");
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationRevokedAsync(_defaultTenantId, user, null, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Permission field is required"));
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationGrantedAsync_Permission_Has_No_Identifier_Returns_ValidationException_Identifier_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId("Context1", "ID1");
            var permission = new Permission(null, "Application1", "Description1");

            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationGrantedAsync(_defaultTenantId, user, permission, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Id field is required"));
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationRevokedAsync_Permission_Has_No_Identifier_Returns_ValidationException_Identifier_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId("Context1", "ID1");
            var permission = new Permission(null, "Application1", "Description1");

            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationRevokedAsync(_defaultTenantId, user, permission, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Id field is required"));
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationGrantedAsync_Permission_Has_EmptyString_Identifier_Returns_ValidationException_Identifier_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId("Context1", "ID1");
            var permission = new Permission(String.Empty, "Application1", "Description1");

            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationGrantedAsync(_defaultTenantId, user, permission, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Id field is required"));
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationRevokedAsync_Permission_Has_EmptyString_Identifier_Returns_ValidationException_Identifier_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId("Context1", "ID1");
            var permission = new Permission(String.Empty, "Application1", "Description1");

            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationRevokedAsync(_defaultTenantId, user, permission, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Id field is required"));
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationGrantedAsync_Permission_Has_No_Application_Returns_ValidationException_Application_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId("Context1", "ID1");
            var permission = new Permission("Identifier01", null, "Description1");

            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationGrantedAsync(_defaultTenantId, user, permission, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Application field is required"));
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationRevokedAsync_Permission_Has_No_Application_Returns_ValidationException_Application_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId("Context1", "ID1");
            var permission = new Permission("Identifier01", null, "Description1");

            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationRevokedAsync(_defaultTenantId, user, permission, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Application field is required"));
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationGrantedAsync_Permission_Has_EmptyString_Application_Returns_ValidationException_Application_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId("Context1", "ID1");
            var permission = new Permission("Identifier01", String.Empty, "Description1");

            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationGrantedAsync(_defaultTenantId, user, permission, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Application field is required"));
        }

        [TestMethod]
        public void PushCompanyWideEffectiveAuthorizationRevokedAsync_Permission_Has_EmptyString_Application_Returns_ValidationException_Application_Field_Required()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId("Context1", "ID1");
            var permission = new Permission("Identifier01", String.Empty, "Description1");

            var pushClient = new PushClient(eventPusherMock.Object);

            //Act & Assert
            var valException = Assert.ThrowsExceptionAsync<ValidationException>(() => pushClient.PushCompanyWideEffectiveAuthorizationRevokedAsync(_defaultTenantId, user, permission, new DateTime(2018, 1, 1)));
            Assert.AreEqual(true, valException.Result.Message.Contains("Application field is required"));
        }

        [TestMethod]
        public async Task PushCompanyWideEffectiveAuthorizationGrantedAsync_All_Data_Correct_IEventPusher_PushGrantedAsync()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId("Context1", "ID1");
            var permission = new Permission("Identifier01", "Application", "Description1");
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act
            await pushClient.PushCompanyWideEffectiveAuthorizationGrantedAsync(_defaultTenantId, user, permission, new DateTime(2018, 1, 1));

            //Assert
            eventPusherMock.Verify(m => m.PushGrantedAsync(It.IsAny<EffectiveAuthorizationEvent>()), Times.Once);
        }

        [TestMethod]
        public async Task PushCompanyWideEffectiveAuthorizationRevokedAsync_All_Data_Correct_IEventPusher_PushRevokedAsync()
        {
            //Arrange
            Mock<IEventPusher<EffectiveAuthorizationEvent>> eventPusherMock = new Mock<IEventPusher<EffectiveAuthorizationEvent>>();
            var user = new ExternalId("Context1", "ID1");
            var permission = new Permission("Identifier01", "Application", "Description1");
            var pushClient = new PushClient(eventPusherMock.Object);

            //Act
            await pushClient.PushCompanyWideEffectiveAuthorizationRevokedAsync(_defaultTenantId, user, permission, new DateTime(2018, 1, 1));

            //Assert
            eventPusherMock.Verify(m => m.PushRevokedAsync(It.IsAny<EffectiveAuthorizationEvent>()), Times.Once);
        }
    }
}
