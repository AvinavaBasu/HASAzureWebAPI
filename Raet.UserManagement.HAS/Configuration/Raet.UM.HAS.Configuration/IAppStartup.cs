using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Raet.UM.HAS.Configuration
{
    public interface IAppStartup
    {
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);

        void ConfigureStartup(IServiceProvider serviceProvider);
    }
}
