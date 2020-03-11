using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raet.UM.HAS.Core.Application;
using Raet.UM.HAS.Core.InitialLoad.Business;
using Raet.UM.HAS.Core.InitialLoad.Interface;
using Raet.UM.HAS.Crosscutting.EventBus;
using Raet.UM.HAS.Crosscutting.EventBus.EventGrid;
using Raet.UM.HAS.Infrastructure.Storage.Blob;
using Raet.UM.HAS.Infrastructure.Storage.CosmosDB;
using System.Collections.Generic;

[assembly: FunctionsStartup(typeof(Raet.UM.HAS.Server.InitialLoadFunction.StartUp))]

namespace Raet.UM.HAS.Server.InitialLoadFunction
{
    public class StartUp : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            IConfiguration configuration = builder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            builder.Services.AddSingleton<IBLOBStorageSettings, BlobStorageSettings>
                 (x => configuration.GetSection("InitialLoadBlobStorageSettings").Get<BlobStorageSettings>())
             .AddSingleton<ICosmoDBSettings, CosmoDBSettings>(e => configuration.GetSection("CosmosReadDb").Get<CosmoDBSettings>())
             .AddSingleton<ICosmoDBStorageInitializer, CosmoDBStorageInitializer>()
             .AddCustomLogging()
             .AddScoped<IInitialLoadBusiness, InitialLoadBusiness>()
             .AddScoped<IAzureBlobStorageInitializer, AzureBlobStorageInitializer>()
             .AddScoped<IAzureBlobStorageRepository, AzureBlobStorageRepository>()
             .AddSingleton<IWriteRawEventStorage, RawEventStorageRepository>()
             .AddSingleton<IEventGridTopicSettings, EventGridTopicSettings>(e =>
                new EventGridTopicSettings(configuration["EventGrid:TopicEndpoint"],
                                       configuration["EventGrid:SasKey"])
             )
             .AddTransient<IEventBus, EventGridBus>(
                p => new EventGridBus(
                    new Dictionary<string, EventGridTopicSettings>{
                       { Topics.EFFECTIVE_AUTHORIZATION_EVENT_STORED, new EventGridTopicSettings(configuration["EventGrid:TopicEndpoint"],
                                                                                                 configuration["EventGrid:SasKey"]) }
                }
             ));


            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ExternalIdProfile>();
                cfg.AddProfile<PermissionProfile>();
                cfg.AddProfile<EffectiveAuthorizationProfile>();
                cfg.AddProfile<EffectiveAuthorizationGrantedEventProfile>();
                cfg.AddProfile<EffectiveAuthorizationRevokedEventProfile>();
            });

        }
    }
}
