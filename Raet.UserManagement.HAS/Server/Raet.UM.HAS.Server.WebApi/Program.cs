using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Raet.UM.HAS.Configuration;

// TODO: using Mock for testing purposes!!

namespace Raet.UM.HAS.Server.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
       {
            AppStartupFactory.Instance = new DefaultAppStartupFactory(Environment.GetEnvironmentVariable("APP_STARTUP_CONFIGURATION")?? nameof(WriteStackAppStartup));
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .UseStartup<Startup>()
                .Build();
    }
}
