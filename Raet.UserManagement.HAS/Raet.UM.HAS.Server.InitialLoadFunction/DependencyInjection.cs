using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Raet.UM.HAS.Server.InitialLoadFunction
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConfigurations<T>(this IServiceCollection sc, IConfiguration configuration,
            string key) where T : class
        {
            var t = configuration.GetSection(key).Get<T>();
            sc.Configure<T>(x => x = t);
            return sc;
        }

        public static IServiceCollection AddCustomLogging(this IServiceCollection sc)
        {
            sc.AddSingleton<ILogger>(c =>
            {
                var factory = c.GetRequiredService<ILoggerFactory>();
                var log = new Logger<InitialLoadFunction>(factory);
                return log;
            });
            return sc;
        }
    }
}
