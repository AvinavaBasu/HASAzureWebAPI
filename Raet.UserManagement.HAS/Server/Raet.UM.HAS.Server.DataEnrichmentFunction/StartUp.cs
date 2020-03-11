using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raet.UM.HAS.Core.Reporting.Business;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting;
using Raet.UM.HAS.Core.Reporting.Helper;
using Raet.UM.HAS.Infrastructure.Storage.CosmosDB;
using Raet.UM.HAS.Infrastructure.Storage.Table;
using Raet.UM.HAS.Infrastructure.Storage.Table.Model;


[assembly: FunctionsStartup(typeof(Raet.UM.HAS.Server.DataEnrichmentFunction.StartUp))]

namespace Raet.UM.HAS.Server.DataEnrichmentFunction
{
    public class StartUp : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            IConfiguration configuration = builder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();
         
            builder.Services
                .AddCustomLogging()
                .AddConfigurations<CosmoDBSettings>(configuration, "CosmosReadDb")
                .AddConfigurations<CosmoDBSettings>(configuration, "CosmosWriteDb")
                .AddSingleton<ITableStorageSettings>(x => configuration.GetSection("TableStorageSettings").Get<TableStorageSettings>())
                .AddSingleton<IAzureTableStorageInitializer, AzureTableStorageInitializer>()
                
                .AddCustomTableStorage<StoredPersonalInfo>()
                .AddCustomTableStorage<ContextMapping>()
                .AddStorageRepository(configuration)
                
                .AddSingleton<IRestSharp, RestSharpHelper>()
                .AddSingleton<ICosmoDBStorageInitializer, CosmoDBStorageInitializer>()
                .AddSingleton<IEffectiveAuthorizationHandlerFactory>(s =>
                {
                    var factory = new EffectiveAuthorizationHandlerFactory();
                    factory.RegisterHandler(typeof(EffectiveAuthorizationGrantedEvent), new PermissionGrantedHandler());
                    factory.RegisterHandler(typeof(EffectiveAuthorizationRevokedEvent), new PermissionRevokedHandler());
                    return factory;
                })
                .AddScoped<IPersonLocalStorage, PersonLocalStorage>()
                .AddScoped<IContextMappingLocalStorage, ContextMappingLocalStorage>()
                .AddScoped<IEffectiveAuthorizationTimelineFactory, EffectiveAuthorizationTimelineFactory>()
                
                .AddScoped<IDataEnrichmentBusiness, DataEnrichmentBusiness>()
                .AddScoped<IUserInformation, UserInformation>();
                

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
