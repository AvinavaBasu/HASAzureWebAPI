using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raet.UM.HAS.Core.Application;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Mocks;

namespace Raet.UM.HAS.Core.Tests.Integration
{
    [TestClass]
    public class HybridEffecitveAuthorizationLoggingTests
    {
        
        [TestMethod]
        public async Task TestHappyFlow()
        {

            // Configure system and DI
            var serviceCollection = new ServiceCollection();
            var appStartup = new MockAppStartupFactory("hybrid").GetAppStartup();
            
            appStartup.ConfigureServices(serviceCollection, null);
            var provider = serviceCollection.BuildServiceProvider();
            appStartup.ConfigureStartup(provider);

            var effAuthzLogginService = provider.GetService<IEffectiveAuthorizationLogging>();

            var grantTime1 = DateTime.UtcNow - TimeSpan.FromDays(4);
            var revokeTime1 = DateTime.UtcNow - TimeSpan.FromDays(3);
            var grantTime2 = DateTime.UtcNow - TimeSpan.FromDays(1);
            var revokeTime2 = DateTime.UtcNow;
                
            var effectiveAuthorization = new EffectiveAuthorization
            {
                User = new ExternalId {Context = "Youforce", Id = "IC000001" },
                Target = new ExternalId {Context = "Youforce", Id = "IC000002" },
                TenantId = "tenant1",
                Permission = new Permission {Application = "A1", Id = "p1"}
            };

            var effAuthzGrantedEvent = new EffectiveAuthorizationGrantedEvent
            {
                EffectiveAuthorization = effectiveAuthorization,
                From = grantTime1
            };
            var effAuthzRevokedEvent = new EffectiveAuthorizationRevokedEvent
            {
                EffectiveAuthorization = effectiveAuthorization,
                Until = revokeTime1
            };

            await effAuthzLogginService.AddAuthLogAsync(effAuthzGrantedEvent);
            await effAuthzLogginService.AddAuthLogAsync(effAuthzRevokedEvent);
            var effAuthzGrantedEvent2 = new EffectiveAuthorizationGrantedEvent
            {
                EffectiveAuthorization = effectiveAuthorization,
                From = grantTime2
            };
            var effAuthzRevokedEvent2 = new EffectiveAuthorizationRevokedEvent
            {
                EffectiveAuthorization = effectiveAuthorization,
                Until = revokeTime2
            };
            await effAuthzLogginService.AddAuthLogAsync(effAuthzGrantedEvent2);
            await effAuthzLogginService.AddAuthLogAsync(effAuthzRevokedEvent2);


            // Retrieve data
            var reportStore = provider.GetService<IReportingStorage>();
            var eventStorage = provider.GetService<IReadRawEventStorage>();
            var intervals = await reportStore.GetIntervals(effectiveAuthorization);
            var events = await eventStorage.GetRawEventsAsync(effectiveAuthorization);

            Assert.AreEqual(4, events.Count);
            Assert.AreEqual(2, intervals.Count);

            // Assert on interval correctnes
            var interval = intervals.Single(i => i.EffectiveInterval.Start == grantTime1);
            Assert.AreEqual(revokeTime1, interval.EffectiveInterval.End);
            interval = intervals.Single(i => i.EffectiveInterval.Start == grantTime2);
            Assert.AreEqual(revokeTime2, interval.EffectiveInterval.End);

        }
    }
}