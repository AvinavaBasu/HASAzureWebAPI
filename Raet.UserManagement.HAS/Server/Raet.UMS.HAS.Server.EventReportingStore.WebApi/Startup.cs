using System;
using System.Collections.Generic;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raet.UM.HAS.Infrastructure.Storage.CosmosDB;
using Swashbuckle.AspNetCore.Swagger;
using VismaRaet.API.AspNetCore.Authorization;

namespace Raet.UMS.HAS.Server.EventReportingStore.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var hangFireDBSettings = Configuration.GetSection("CosmosHangFireDb").Get<CosmoDBSettings>();
            services.AddHangfire(x => x.UseAzureDocumentDbStorage(hangFireDBSettings.Endpoint, hangFireDBSettings.AuthKey, hangFireDBSettings.Database, hangFireDBSettings.Collection));
            services.AddHangfireServer();

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddApplicationInsightsTelemetry();

            services.AddConfigurations(Configuration).AddCosmosDbSettings()
                .AddBlobStorageSettings()
                .AddTableStorageSettings()
                .AddQueueStorageSettings()
                .AddOtherDependencies()
                .ConfigureAccess(Configuration)
                .AddInitialLoadDependency(Configuration)
                .AddSwaggerGen(c =>
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
                        Title = "Raet Historical Authorization Store Enriched Data WebApi",
                        Version = "v1",
                        Description = "ASP.NET Core Web API for fetching Raet Historical Authorization Store Reporting Events",
                        Contact = new Contact { Name = "Black Panthers team" }
                    });
                    c.OperationFilter<FormFileSwaggerFilter>();
                    c.MapType<DateTime?>(() => new Schema { Example = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:sszzz") });
                })
                .AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters()
                .AddApiExplorer()
                .AddDataAnnotations();
            
            //services.ConfigureAuthorization();
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



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "My API (V 1.0)");
                c.RoutePrefix = string.Empty;
            });
            
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseHangfireDashboard(); 
            app.UseMvc();
        }
    }
}
