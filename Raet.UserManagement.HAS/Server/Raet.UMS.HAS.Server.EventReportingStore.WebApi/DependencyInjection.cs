using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Raet.UM.HAS.Core.InitialLoad.Business;
using Raet.UM.HAS.Core.InitialLoad.Interface;
using Raet.UM.HAS.Core.Reporting.Business;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Infrastructure.Storage.Blob;
using Raet.UM.HAS.Infrastructure.Storage.CosmosDB;
using Raet.UM.HAS.Infrastructure.Storage.Queue;
using Raet.UM.HAS.Infrastructure.Storage.Table;
using Raet.UM.HAS.Infrastructure.Storage.Table.Model;
using Raet.UMS.HAS.Server.EventReportingStore.WebApi.Controllers;
using VismaRaet.API.Authorization.Core;
using VismaRaet.API.Authorization.Core.AccessCheckers;
using VismaRaet.API.Authorization.Core.Clients;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authorization;
using VismaRaet.API.AspNetCore.Authorization;
using Raet.Identity.Middleware.NetCore.Jwt;
using Raet.UM.HAS.Core.Application;
using Raet.UM.HAS.Core.Reporting.Helper;
using Raet.UM.HAS.Core.Reporting;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Infrastructure.Storage.Table.Storages;
using Microsoft.WindowsAzure.Storage.Table;

namespace Raet.UMS.HAS.Server.EventReportingStore.WebApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<ICosmoDBSettings, CosmoDBSettings>
                (x => configuration.GetSection("CosmoDBSettings").Get<CosmoDBSettings>());
            services.AddSingleton<ITableStorageSettings, TableStorageSettings>
                (x => configuration.GetSection("TableStorageSettings").Get<TableStorageSettings>());
            services.AddSingleton<IBLOBStorageSettings, BlobStorageSettings>
                (x => configuration.GetSection("InitialLoadBlobStorageSettings").Get<BlobStorageSettings>());
            services.AddSingleton<IQueueStorageSettings, QueueStorageSettings>
                (x => configuration.GetSection("GenerateReportQueueStorageSettings").Get<QueueStorageSettings>());

            return services;
        }

        public static IServiceCollection AddCosmosDbSettings(this IServiceCollection services)
        {

            services.AddScoped<ICosmoDBStorageInitializer, CosmoDBStorageInitializer>();
            services.AddScoped<IReportingStorage, ReportingStorageRepository>();

            return services;
        }

        public static IServiceCollection AddTableStorageSettings(this IServiceCollection services)
        {

            services.AddScoped<IAzureTableStorageInitializer, AzureTableStorageInitializer>();
            services.AddScoped<IAzureTableStorageRepository<FileInformation>, AzureTableStorageRepository<FileInformation>>();
            return services;
        }


        public static IServiceCollection AddBlobStorageSettings(this IServiceCollection services)
        {

            services.AddScoped<IAzureBlobStorageInitializer, AzureBlobStorageInitializer>();
            services.AddScoped<IAzureBlobStorageRepository, AzureBlobStorageRepository>();
            return services;
        }

        public static IServiceCollection AddQueueStorageSettings(this IServiceCollection services)
        {

            services.AddScoped<IAzureQueueStorageInitializer, AzureQueueStorageInitializer>();
            services.AddScoped<IAzureQueueStorageRepository, AzureQueueStorageRepository>();
            return services;
        }

        public static IServiceCollection ConfigureAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddPingJwtBearer(configuration.GetSection("PingFederate").GetValue<string>("Jwks.Url"), configuration.GetSection("PingFederate").GetValue<string>("Jwks.SigningKey"));


            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
            services.AddSingleton<IAuthorizationRequirement, PermissionRequirement>();
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

            var adalConfiguration = new AdalConfiguration()
            {
                Audience = configuration.GetSection("AdalConfigurtaion").GetValue<string>("Audience"),// "https://youforceonedev.onmicrosoft.com/youforceone-api",
                ClientId = configuration.GetSection("AdalConfigurtaion").GetValue<string>("ClientId"),// "5cc2c606-a3e6-4e46-9bdc-b07c231374d2",
                ClientSecret = configuration.GetSection("AdalConfigurtaion").GetValue<string>("ClientSecret"),//"Hru1:^m+UVAp{QvR8|FrOZfdRy",
                Tenant = configuration.GetSection("AdalConfigurtaion").GetValue<string>("Tenant"),//"youforceonedev.onmicrosoft.com",
                AdInstance = configuration.GetSection("AdalConfigurtaion").GetValue<string>("AdInstance")//"https://login.microsoftonline.com"
            };
            var umsUrl = configuration.GetSection("Ums").GetValue<string>("umsUrl");// "https://yfo-greyjoy-dev-ums.azurewebsites.net/api/v2.0";
            services.AddSingleton<ITokenProvider>(new AdalTokenProvider(adalConfiguration));
            services.AddSingleton<IApiClient>(_ => new RestApiClient(umsUrl, _.GetService<ITokenProvider>()));
            services.AddSingleton<IPermissionProvider, UmsPermissionProvider>();
            services.AddSingleton<IAccessChecker, PingUserTokenAccessChecker>();
            services.AddSingleton<IPermissionChecker, DefaultPermissionChecker>();
            return services;
        }

        public static IServiceCollection AddOtherDependencies(this IServiceCollection services)
        {
            services.AddScoped<ILogger>(c =>
            {
                var factory = c.GetRequiredService<ILoggerFactory>();
                var log = new Logger<ReportController>(factory);
                return log;
            });

            services.AddScoped<ILogger>(c =>
            {
                var factory = c.GetRequiredService<ILoggerFactory>();
                var log = new Logger<ReportDetailController>(factory);
                return log;
            });

            services.AddSingleton<IChecksumGenerator, ChecksumGenerator>();
           
            services.AddScoped<IEAAggregateBusiness, EAAggregateBusiness>();
            return services;
        }

        public static IServiceCollection AddCustomTableStorage<T>(this IServiceCollection sc) where T : TableEntity
        {
            sc.AddScoped(x => new AzureTableStorage<T>(x.GetRequiredService<ITableStorageSettings>()))

            .AddScoped<IAzureTableStorageRepository<T>>(c =>
            {
                var log = c.GetRequiredService<ILogger>();
                return c.GetRequiredService<AzureTableStorage<T>>().GetInstance(log);
            });
            return sc;
        }

        public static IServiceCollection AddInitialLoadDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IInitialLoadFileUploadBusiness, InitialLoadFileUploadBusiness>();
            services.AddScoped<IInitialLoadBusiness, InitialLoadBusiness>()
            .AddCustomTableStorage<StoredPersonalInfo>()
               .AddCustomTableStorage<ContextMapping>();

            services.AddSingleton<IRestSharp, RestSharpHelper>()
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
               .AddScoped<IUserInformation, UserInformation>();

            services.AddScoped<IWriteRawEventStorage>(c =>
            {
                var cosmosDbSettings = configuration.GetSection("CosmosReadDb").Get<CosmoDBSettings>();
                var log = c.GetRequiredService<ILogger>();
                return new InitialLoadStorageRepository(GetCosmoCollectionSettings(cosmosDbSettings), log);
            });

            services.AddScoped<IReadRawEventStorage>(c =>
            {
                var cosmosDbSettings = configuration.GetSection("CosmosReadDb").Get<CosmoDBSettings>();
                var log = c.GetRequiredService<ILogger>();
                return new RawEventStorageRepository(GetCosmoCollectionSettings(cosmosDbSettings), log);
            });

            services.AddScoped<IReportingStorage>(c =>
            {
                var cosmosDbSettings = configuration.GetSection("CosmoDBSettings").Get<CosmoDBSettings>();
                var log = c.GetRequiredService<ILogger>();
                return new ReportingStorageRepository(GetCosmoCollectionSettings(cosmosDbSettings), log);
            });

            services.AddScoped<IDataEnrichmentBusiness>(c =>
            {
                var cosmosReadDbSettings = configuration.GetSection("CosmosReadDb").Get<CosmoDBSettings>();
                var cosmosWriteDbSettings = configuration.GetSection("CosmoDBSettings").Get<CosmoDBSettings>();
                var log = c.GetRequiredService<ILogger>();
                var logFactory = c.GetRequiredService<ILoggerFactory>();
                var reportingStorage = c.GetRequiredService<IReportingStorage>();
                var effectiveAuthorizationHandlerFactory = c.GetRequiredService<IEffectiveAuthorizationHandlerFactory>();
                var userInformation = c.GetRequiredService<IUserInformation>();
                return new DataEnrichmentBusiness(GetEffectiveAuthorizationTimelineFactory(GetreadRawEventStorage(GetCosmoCollectionSettings(cosmosReadDbSettings), log),
                    effectiveAuthorizationHandlerFactory), GetReportingStorage(GetCosmoCollectionSettings(cosmosWriteDbSettings), log), userInformation, log);
            });

            services.AddScoped<IReportingBusiness>(c =>
            {
                var log = c.GetRequiredService<ILogger>();
                var reportingStorage = c.GetRequiredService<IReportingStorage>();
                var fileInformationRepository = c.GetRequiredService<IAzureTableStorageRepository<FileInformation>>();
                var checksumGenerator = c.GetRequiredService<IChecksumGenerator>();
                var azureQueueStorageRepository = c.GetRequiredService<IAzureQueueStorageRepository>();
                var generateReportBlobStorageSettings = configuration.GetSection("GenerateReportBlobStorageSettings").Get<BlobStorageSettings>();
                return new ReportingBusiness(reportingStorage, GetAzureBlobStorageRepository(GetAzureBlobStorageInitializer(generateReportBlobStorageSettings, log), log),
                    fileInformationRepository, checksumGenerator, generateReportBlobStorageSettings, azureQueueStorageRepository, log);
            });

            return services;
        }

        private static IAzureBlobStorageInitializer GetAzureBlobStorageInitializer(IBLOBStorageSettings bLOBStorageSettings, ILogger logger)
        {
            return new AzureBlobStorageInitializer(bLOBStorageSettings, logger);
        }
        private static IAzureBlobStorageRepository GetAzureBlobStorageRepository(IAzureBlobStorageInitializer azureBlobStorageInitializer,
             ILogger logger)
        {
            return new AzureBlobStorageRepository(azureBlobStorageInitializer, logger);
        }

        private static IAzureQueueStorageInitializer GetAzureQueueStorageInitializer(IQueueStorageSettings queueStorageSettings, ILogger logger)
        {
            return new AzureQueueStorageInitializer(queueStorageSettings, logger);
        }
        private static IAzureQueueStorageRepository GetAzureQueueStorageRepository(IAzureQueueStorageInitializer azureQueueStorageInitializer,
             ILogger logger)
        {
            return new AzureQueueStorageRepository(azureQueueStorageInitializer, logger);
        }

        private static IReportingStorage GetReportingStorage(ICosmoDBStorageInitializer clientInitializer, ILogger factory)
        {
            return new ReportingStorageRepository(clientInitializer, factory);
        }

        private static IReadRawEventStorage GetreadRawEventStorage(ICosmoDBStorageInitializer clientInitializer, ILogger factory)
        {
            return new RawEventStorageRepository(clientInitializer, factory);
        }

        private static IEffectiveAuthorizationTimelineFactory GetEffectiveAuthorizationTimelineFactory(IReadRawEventStorage eventStore,
            IEffectiveAuthorizationHandlerFactory eventHandlerFactory)
        {
            return new EffectiveAuthorizationTimelineFactory(eventStore, eventHandlerFactory);
        }

        private static ICosmoDBStorageInitializer GetCosmoCollectionSettings(
            ICosmoDBSettings settings)
        {
            return new CosmoDBStorageInitializer(settings);
        }

    }
}
