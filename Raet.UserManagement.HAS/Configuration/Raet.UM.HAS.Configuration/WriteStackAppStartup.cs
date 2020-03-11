using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raet.UM.HAS.Core.Application;
using Raet.UM.HAS.Crosscutting.EventBus;
using Raet.UM.HAS.Infrastructure.Storage.CosmosDB;

namespace Raet.UM.HAS.Configuration
{
    public class WriteStackAppStartup : IAppStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEffectiveAuthorizationLogging, WriteStackEffectiveAuthorizationLogging>();
            services.AddSingleton<ICosmoDBSettings, CosmoDBSettings>(e => configuration.GetSection("CosmoDBSettings").Get<CosmoDBSettings>());
            services.AddSingleton<ICosmoDBStorageInitializer, CosmoDBStorageInitializer>();
            services.AddSingleton<IWriteRawEventStorage, RawEventStorageRepository>();
        }
        
            

        public void ConfigureStartup(IServiceProvider serviceProvider)
        {
            // Optional method on NetCore startup, not explictly needed in this startup type
        }
    }
}
