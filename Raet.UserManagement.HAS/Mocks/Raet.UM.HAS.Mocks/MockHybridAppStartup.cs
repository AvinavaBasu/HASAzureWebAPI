using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raet.UM.HAS.Configuration;
using Raet.UM.HAS.Core.Application;
using Raet.UM.HAS.Core.Reporting;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Crosscutting.EventBus;

namespace Raet.UM.HAS.Mocks
{
    public class MockHybridAppStartup : IAppStartup
    {
        public virtual void ConfigureServices(IServiceCollection services,IConfiguration configuration)
        {
            ConfigureMockServicesHelper.ConfigureServices(services);

            services.AddSingleton<IEventBus, MemoryEventBus>();
            services.AddTransient<IEffectiveAuthorizationLogging, HybridEffectiveAuthorizationLogging>();
        }

        public virtual void ConfigureStartup(IServiceProvider serviceProvider)
        {
            var eventBus = serviceProvider.GetService<IEventBus>();
            var dataEnrichmentService = serviceProvider.GetService<IDataEnrichmentService>();

            eventBus.GetTopicEffectiveAuthorizationEventStored()
                .Subscribe(async auth =>
                {
                    await dataEnrichmentService.AddEffectiveAuthorizationAsync(auth);
                });
        }
    }
}