using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Raet.UM.HAS.Core.Application;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Infrastructure.Storage.Table;

namespace Raet.UM.HAS.Mocks
{
    public static class ConfigureMockServicesHelper
    {
        public static void ConfigureServices(IServiceCollection services) 
        {
            var rawEventInMemoryStore = new RawEventInMemoryStorage();
            services.AddSingleton<IWriteRawEventStorage>(provider => rawEventInMemoryStore);
            services.AddSingleton<IReadRawEventStorage>(provider => rawEventInMemoryStore);
            
            services.AddTransient<IDataEnrichmentService, DataEnrichmentService>();

            services.AddTransient<IEffectiveAuthorizationTimelineFactory, EffectiveAuthorizationTimelineFactory>();
            services.AddSingleton<IEffectiveAuthorizationHandlerFactory>(s =>
            {
                var factory = new EffectiveAuthorizationHandlerFactory();
                factory.RegisterHandler(typeof(EffectiveAuthorizationGrantedEvent), new PermissionGrantedHandler());
                factory.RegisterHandler(typeof(EffectiveAuthorizationRevokedEvent), new PermissionRevokedHandler());
                return factory;
            });

            services.AddSingleton<ILogger, MockInMemoryLogger>();
            services.AddTransient<IPersonalInfoEnrichmentService, PersonalInfoEnrichmentService>();
            services.AddTransient<IPersonLocalStorage, MockPersonalLocalStorage>();
            services.AddTransient<IPersonalInfoExternalServiceFactory, MockPersonalInfoExternalServiceFactory>();
            
            services.AddSingleton<IReportingStorage, ReportingInMemoryStorage>();
        }
    }
}