using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raet.UM.HAS.Core.Application;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.InitialLoad.Business;
using Raet.UM.HAS.Core.InitialLoad.Interface;
using Raet.UM.HAS.Core.Reporting;
using Raet.UM.HAS.Core.Reporting.Business;
using Raet.UM.HAS.Core.Reporting.Helper;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Infrastructure.Storage.Blob;
using Raet.UM.HAS.Infrastructure.Storage.CosmosDB;
using Raet.UM.HAS.Infrastructure.Storage.Queue;
using Raet.UM.HAS.Infrastructure.Storage.Table;
using Raet.UM.HAS.Infrastructure.Storage.Table.Model;
using Reat.UM.HAS.Core.GenerateReport.Business;
using Reat.UM.HAS.Core.GenerateReport.Interface;

[assembly: FunctionsStartup(typeof(Raet.UM.HAS.Server.ReatHasGdprFunctionApps.StartUp))]

namespace Raet.UM.HAS.Server.ReatHasGdprFunctionApps
{
    public class StartUp : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            IConfiguration configuration = builder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            builder.Services
               .AddCustomLogging()

            #region DataEnrichment StartUp
               .AddConfigurations<CosmoDBSettings>(configuration, "CosmosReadDb")
               .AddConfigurations<CosmoDBSettings>(configuration, "CosmosWriteDb")
               .AddSingleton<ITableStorageSettings>(x => configuration.GetSection("TableStorageSettings").Get<TableStorageSettings>())
               .AddCustomTableStorage<StoredPersonalInfo>()
               .AddCustomTableStorage<ContextMapping>()
               .AddStorageRepository(configuration)
               .AddSingleton<IRestSharp, RestSharpHelper>()
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
               .AddScoped<IUserInformation, UserInformation>()
            #endregion

            #region GenerateReport StartUp
               .AddSingleton<ITableStorageSettings, TableStorageSettings>(x => configuration.GetSection("TableStorageSettings").Get<TableStorageSettings>())
               .AddSingleton<ICosmoDBSettings, CosmoDBSettings>(x => configuration.GetSection("CosmosWriteDb").Get<CosmoDBSettings>())
               .AddSingleton<IQueueStorageSettings, QueueStorageSettings>(x => configuration.GetSection("QueueStorageSettings").Get<QueueStorageSettings>())
               .AddScoped<IGenerateReportBusiness, GenerateReportBusiness>()
               .AddSingleton<IChecksumGenerator, ChecksumGenerator>()
               .AddScoped<IReportingBusiness, ReportingBusiness>()
               .AddScoped<IAzureQueueStorageInitializer, AzureQueueStorageInitializer>()
               .AddScoped<IAzureQueueStorageRepository, AzureQueueStorageRepository>()
               .AddScoped<IAzureTableStorageRepository<FileInformation>, AzureTableStorageRepository<FileInformation>>()
            #endregion

            #region InitialLoad StartUp
             .AddSingleton<ICosmoDBSettings, CosmoDBSettings>(e => configuration.GetSection("CosmosReadDb").Get<CosmoDBSettings>())
             .AddScoped<IInitialLoadBusiness, InitialLoadBusiness>()
             .AddSingleton<IWriteRawEventStorage, RawEventStorageRepository>()
            #endregion

            #region Shared StartUp
               .AddCustomBlobStorageSettings(configuration)
               .AddSingleton<ICosmoDBStorageInitializer, CosmoDBStorageInitializer>()
               .AddScoped<IAzureBlobStorageInitializer, AzureBlobStorageInitializer>()
               .AddScoped<IAzureBlobStorageRepository, AzureBlobStorageRepository>()
               .AddSingleton<IAzureTableStorageInitializer, AzureTableStorageInitializer>();
            #endregion

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ExternalIdProfile>();
                cfg.AddProfile<PermissionProfile>();
                cfg.AddProfile<EffectiveAuthorizationProfile>();
                cfg.AddProfile<EffectiveAuthorizationGrantedEventProfile>();
                cfg.AddProfile<EffectiveAuthorizationRevokedEventProfile>();
                cfg.AddProfile<EffectiveIntervalCSVMapperProfile>();
            });
        }
    }
}
