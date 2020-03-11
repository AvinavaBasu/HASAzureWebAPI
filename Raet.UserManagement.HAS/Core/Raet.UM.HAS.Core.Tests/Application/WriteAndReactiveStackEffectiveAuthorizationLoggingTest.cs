using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raet.UM.HAS.Core.Application;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Crosscutting.EventBus;
using Raet.UM.HAS.Crosscutting.EventBus.EventGrid;
using Raet.UM.HAS.Mocks;

namespace Raet.UM.HAS.Core.Tests.Application
{
    [TestClass]
    public class WriteAndReactiveStackEffectiveAuthorizationLoggingTest
    {
        private Mock<IWriteRawEventStorage> _writeRawEventStorageMock;

        private WriteStackEffectiveAuthorizationLogging _write;

        public WriteAndReactiveStackEffectiveAuthorizationLoggingTest()
        {
            _writeRawEventStorageMock = new Mock<IWriteRawEventStorage>();
        }

        [TestMethod]
        public void WriteStackEffectiveAuthorizationLoggingTest()
        {
            //Arrange
            _write = new WriteStackEffectiveAuthorizationLogging(_writeRawEventStorageMock.Object);

            EffectiveAuthorizationEvent authorizationEvent = new EffectiveAuthorizationGrantedEvent();

            _writeRawEventStorageMock.Setup(e => e.WriteRawEventAsync(authorizationEvent))
                .Returns(Task.FromResult<string>("WriteTestData"));

            //Act
            var result = _write.AddAuthLogAsync(authorizationEvent);

            //Assert
            Assert.AreEqual("WriteTestData", result.Result);

        }

        [TestMethod]
        public void ReactiveEffectiveAuthorizationLoggingTestAsync()
        {
            //Arrange

            var serviceCollection = new ServiceCollection();
            var appStartup = new MockAppStartupFactory("reactive").GetAppStartup();

            appStartup.ConfigureServices(serviceCollection, null);
            var provider = serviceCollection.BuildServiceProvider();
            appStartup.ConfigureStartup(provider);

            var effAuthLoginService = provider.GetService<IEffectiveAuthorizationLogging>();

            var grantTime1 = DateTime.UtcNow - TimeSpan.FromDays(4);

            var effectiveAuthorization = new EffectiveAuthorization
            {
                User = new ExternalId { Context = "Youforce", Id = "IC000001" },
                Target = new ExternalId { Context = "Youforce", Id = "IC000002" },
                TenantId = "tenant1",
                Permission = new Permission { Application = "A1", Id = "p1" }
            };

            var effAuthzGrantedEvent = new EffectiveAuthorizationGrantedEvent
            {
                EffectiveAuthorization = effectiveAuthorization,
                From = grantTime1
            };

            //Act
            var result = effAuthLoginService.AddAuthLogAsync(effAuthzGrantedEvent);

            //Assert
            Assert.AreEqual("RanToCompletion", result.Status.ToString());
        }

    }
}
