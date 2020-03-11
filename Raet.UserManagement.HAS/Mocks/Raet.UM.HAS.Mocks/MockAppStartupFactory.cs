using System;
using Raet.UM.HAS.Configuration;

namespace Raet.UM.HAS.Mocks
{
    public class MockAppStartupFactory : IAppStartupFactory
    {
        private readonly string configuration;
        private IAppStartup startup;

        public MockAppStartupFactory(string configuration)
        {
            this.configuration = configuration.ToLower();
        }

        public IAppStartup GetAppStartup()
        {
            if (startup != null) return startup;
            switch (configuration)
            {
                case "hybrid":
                    startup = new MockHybridAppStartup();
                    break;
                case "reactive":
                    startup = new MockReactiveAppStartup();
                    break;
                default: throw new Exception("Invalid configuration");
            }
            return startup;
        }
    }
}
