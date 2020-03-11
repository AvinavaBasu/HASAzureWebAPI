using System;

namespace Raet.UM.HAS.Configuration
{
    public class DefaultAppStartupFactory : IAppStartupFactory
    {
        private readonly string configuration;
        private IAppStartup startup;

        public DefaultAppStartupFactory(string configuration)
        {
            this.configuration = configuration;
        }

        public IAppStartup GetAppStartup()
        {
            if (startup != null) return startup;
            switch (configuration)
            {
                // Different implementations of startup
                case nameof(HybridAppStartup):
                    startup = new HybridAppStartup();
                    break;
                case nameof(WriteStackAppStartup):
                    startup= new WriteStackAppStartup();
                    break;
                case nameof(ReactiveAppStartup):
                    startup = new ReactiveAppStartup();
                    break;
                default: throw new Exception("Invalid configuration");
            }
            return startup;
        }
    }
}
