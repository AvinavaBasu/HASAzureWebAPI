using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using Raet.API.Infrastructure.Tracing.NetCore;
using Raet.Identity.Middleware.NetCore.Jwt;
using Raet.UM.HAS.Configuration;
using Raet.UM.HAS.Infrastructure.Storage.CosmosDB;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using VismaRaet.API.AspNetCore.Authorization;

namespace Raet.UM.HAS.Server.WebApi
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;

        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env, IConfiguration configuration) {
            _env = env;
            _env.ConfigureNLog("nlog.config");
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            AppStartupFactory.Instance.GetAppStartup().ConfigureServices(services, Configuration);

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    In = "header",
                    Name = "Authorization",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } } });

                c.AddSecurityDefinition("TenantId", new ApiKeyScheme
                {
                    Description = "Provide the Tenant-Id",
                    In = "header",
                    Name = "x-raet-tenant-id",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> { { "TenantId", new string[] { } } });

                c.SwaggerDoc("v1.0", new Info
                {
                    Title = "Raet Historical Authorization Store WebApi",
                    Version = "v1",
                    Description = "ASP.NET Core Web API for Raet Historical Authorization Store",
                    Contact = new Contact { Name = "Van der Something team", Email = "vandersomethingemail@raet.com," }
                });
                c.MapType<DateTime?>(() => new Schema { Example = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:sszzz") });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddPingJwtBearer(Configuration["PingFederate:Jwks.EndPoint"], Configuration["PingFederate:Jwks.KeyId"]);
            services.ConfigureAuthorization();
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ExternalIdProfile>();
                cfg.AddProfile<PermissionProfile>();
                cfg.AddProfile<EffectiveAuthorizationProfile>();
                cfg.AddProfile<EffectiveAuthorizationGrantedEventProfile>();
                cfg.AddProfile<EffectiveAuthorizationRevokedEventProfile>();
            });

            services.AddTracingService(Configuration);
            services.AddMvcCore()
                    .AddAuthorization()
                    .AddJsonFormatters()
                    .AddApiExplorer()
                    .AddDataAnnotations();
            services.AddApplicationInsightsTelemetry();

        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            AppStartupFactory.Instance.GetAppStartup().ConfigureStartup(app.ApplicationServices);

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddNLog();
            loggerFactory.AddAzureWebAppDiagnostics();


            app.UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "My API (V 1.0)");
                c.RoutePrefix = string.Empty;
            });

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseTracing(Configuration);
            app.UseMvc();
        }
    }
}
