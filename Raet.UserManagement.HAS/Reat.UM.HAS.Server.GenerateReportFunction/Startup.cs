using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raet.UM.HAS.Core.Reporting.Business;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Infrastructure.Storage.Blob;
using Raet.UM.HAS.Infrastructure.Storage.CosmosDB;
using Raet.UM.HAS.Infrastructure.Storage.Queue;
using Raet.UM.HAS.Infrastructure.Storage.Table;
using Raet.UM.HAS.Infrastructure.Storage.Table.Model;
using Reat.UM.HAS.Core.GenerateReport.Business;
using Reat.UM.HAS.Core.GenerateReport.Interface;

[assembly: FunctionsStartup(typeof(Reat.UM.HAS.Server.GenerateReportFunction.StartUp))]

namespace Reat.UM.HAS.Server.GenerateReportFunction
{
    public class StartUp : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            IConfiguration configuration = builder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            builder.Services.AddCustomLogging()
                .AddSingleton<IBLOBStorageSettings, BlobStorageSettings>(x => configuration.GetSection("GenerateReportBlobStorageSettings").Get<BlobStorageSettings>())
                .AddSingleton<ITableStorageSettings, TableStorageSettings>(x => configuration.GetSection("TableStorageSettings").Get<TableStorageSettings>())
                .AddSingleton<ICosmoDBSettings, CosmoDBSettings>(x => configuration.GetSection("CosmosWriteDb").Get<CosmoDBSettings>())
                .AddSingleton<IQueueStorageSettings, QueueStorageSettings>(x => configuration.GetSection("QueueStorageSettings").Get<QueueStorageSettings>())
                .AddScoped<ICosmoDBStorageInitializer, CosmoDBStorageInitializer>()
                .AddScoped<IGenerateReportBusiness, GenerateReportBusiness>()
                .AddScoped<IAzureBlobStorageInitializer, AzureBlobStorageInitializer>()
                .AddScoped<IAzureBlobStorageRepository, AzureBlobStorageRepository>()
                .AddSingleton<IChecksumGenerator, ChecksumGenerator>()
                .AddScoped<IReportingBusiness, ReportingBusiness>()
                .AddScoped<IReportingStorage, ReportingStorageRepository>()
                .AddScoped<IAzureTableStorageInitializer, AzureTableStorageInitializer>()
                .AddScoped<IAzureQueueStorageInitializer, AzureQueueStorageInitializer>()
                .AddScoped<IAzureQueueStorageRepository, AzureQueueStorageRepository>()
                .AddScoped<IAzureTableStorageRepository<FileInformation>, AzureTableStorageRepository<FileInformation>>();


            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<EffectiveIntervalCSVMapperProfile>();
            });
        }
    }
}