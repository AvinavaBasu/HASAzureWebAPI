using Microsoft.Extensions.DependencyInjection;
using System;
using Raet.UM.HAS.Core.Application;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting;
using Raet.UM.HAS.Crosscutting.EventBus;
using Raet.UM.HAS.Infrastructure.Storage.CosmosDB;
using Microsoft.Extensions.Configuration;
using Raet.UM.HAS.Core.Reporting.Interface;

namespace Raet.UM.HAS.Configuration
{
    public class HybridAppStartup : IAppStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEventBus, MemoryEventBus>();
            services.AddTransient<IEffectiveAuthorizationLogging, HybridEffectiveAuthorizationLogging>();
            
            // TODO: discuss whether this is correct. In particular, configuration is being build every single time the
            //       a request is created....
            services.AddTransient<IWriteRawEventStorage, RawEventStorageRepository>();
            services.AddTransient<IDataEnrichmentService, DataEnrichmentService>();
            services.AddTransient<IEffectiveAuthorizationTimelineFactory, EffectiveAuthorizationTimelineFactory>();
            services.AddSingleton<IEffectiveAuthorizationHandlerFactory>(s =>
            {
                var factory = new EffectiveAuthorizationHandlerFactory();
                factory.RegisterHandler(typeof(EffectiveAuthorizationGrantedEvent), new PermissionGrantedHandler());
                factory.RegisterHandler(typeof(EffectiveAuthorizationRevokedEvent), new PermissionRevokedHandler());
                return factory;
            });

            services.AddTransient<IPersonalInfoEnrichmentService, PersonalInfoEnrichmentService>();
            // TODO: Missing implementations
        }

        public void ConfigureStartup(IServiceProvider serviceProvider)
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
