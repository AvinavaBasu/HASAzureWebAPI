using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog.Targets;
using Raet.UM.HAS.Core.Application;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Server.WebApi.Controllers;
using Raet.UM.HAS.Crosscutting.Exceptions;
using System.Net.Http;

namespace Raet.UM.HAS.Server.WebApi.Tests
{
    [TestClass]
    public class EffectiveAuthorizationControllerTests
    {
        private Mock<IEffectiveAuthorizationLogging> mEffectiveAuthorizationLogging;
        private Mock<ILoggerFactory> mLoggerFactory;
        private EffectiveAuthorizationEventsController cont;

        [TestInitialize]
        public void SetupMocks()
        {
            mEffectiveAuthorizationLogging = new Mock<IEffectiveAuthorizationLogging>();
            mEffectiveAuthorizationLogging.Setup(e => e.AddAuthLogAsync(It.IsNotNull<EffectiveAuthorizationEvent>())).Returns(Task.FromResult<string>("123456"));
            mLoggerFactory = new Mock<ILoggerFactory>();
            cont = new EffectiveAuthorizationEventsController(mEffectiveAuthorizationLogging.Object, mLoggerFactory.Object)
            {
                ControllerContext = new ControllerContext()
            };
        }

        [TestMethod]
        public void LogEffectiveAutorizationGrantedEvent_WithProperEvent_Returns201CreatedResult()
        {
            var autorizationEvent = (DTOs.EffectiveAuthorizationGrantedEvent)EventFactory.BuildEventByType("granted");
            var sut = cont.LogEffectiveAutorizationGrantedEvent(autorizationEvent).Result;
            Assert.IsNotNull(sut);
            Assert.IsInstanceOfType(sut, typeof(CreatedResult));
            
        }

        [TestMethod]
        public void LogEffectiveAutorizationGrantedEvent_WithProperEvent_CallsAddAuthLogAsync()
        {
            var autorizationEvent = (DTOs.EffectiveAuthorizationGrantedEvent)EventFactory.BuildEventByType("granted");
            var sut = cont.LogEffectiveAutorizationGrantedEvent(autorizationEvent).Result;
            mEffectiveAuthorizationLogging.Verify(e => e.AddAuthLogAsync(It.IsAny<EffectiveAuthorizationGrantedEvent>()), Times.Once);
        }
   


        [TestMethod]
        public void LogEffectiveAutorizationRevokedEvent_WithProperEvent_Returns201CreatedResult()
        {
            var autorizationEvent = (DTOs.EffectiveAuthorizationRevokedEvent)EventFactory.BuildEventByType("revoked");
            var sut = cont.LogEffectiveAutorizationRevokedEvent(autorizationEvent).Result;

            Assert.IsNotNull(sut);
            Assert.IsInstanceOfType(sut, typeof(CreatedResult));

        }

        [TestMethod]
        public void LogEffectiveAutorizationRevokedEvent_WithProperEvent_CallsAddAuthLogAsync()
        {
            var autorizationEvent = (DTOs.EffectiveAuthorizationRevokedEvent)EventFactory.BuildEventByType("revoked");
            var sut = cont.LogEffectiveAutorizationRevokedEvent(autorizationEvent).Result;
            mEffectiveAuthorizationLogging.Verify(e => e.AddAuthLogAsync(It.IsAny<EffectiveAuthorizationRevokedEvent>()), Times.Once);
        }
    }
}
