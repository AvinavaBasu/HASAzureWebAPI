using Microsoft.Extensions.DependencyInjection;
using System;
using Raet.UM.HAS.Core.Application;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting;
using Raet.UM.HAS.Crosscutting.EventBus;
using Raet.UM.HAS.Infrastructure.Storage.CosmosDB;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Raet.UM.HAS.Crosscutting.EventBus.EventGrid;


namespace Raet.UM.HAS.Configuration
{
    public class ReactiveAppStartup : IAppStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // TODO: discuss whether this is correct. In particular, configuration is being build every single time the
            //       a request is created....

            services.AddSingleton<ICosmoDBSettings, CosmoDBSettings>(e => configuration.GetSection("CosmoDBSettings").Get<CosmoDBSettings>());
            services.AddSingleton<ICosmoDBStorageInitializer, CosmoDBStorageInitializer>();
            services.AddSingleton<IWriteRawEventStorage, RawEventStorageRepository>();
            services.AddTransient<IEffectiveAuthorizationLogging, ReactiveEffectiveAuthorizationLogging>();

            services.AddSingleton<IEventGridTopicSettings, EventGridTopicSettings>(e =>
               new EventGridTopicSettings(configuration["EventGrid:TopicEndpoint"],
                                      configuration["EventGrid:SasKey"])
            );

            services.AddTransient<IEventBus, EventGridBus>(
               p => new EventGridBus( 
                   new Dictionary<string, EventGridTopicSettings>{
                       { Topics.EFFECTIVE_AUTHORIZATION_EVENT_STORED, new EventGridTopicSettings(configuration["EventGrid:TopicEndpoint"],
                                                                                                 configuration["EventGrid:SasKey"]) }
               }
            ));
            
          
        }

        public void ConfigureStartup(IServiceProvider serviceProvider)
        {
            var eventBus = serviceProvider.GetService<IEventBus>();
        }
    }
}
