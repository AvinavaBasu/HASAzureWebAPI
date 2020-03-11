using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raet.UM.HAS.Infrastructure.Storage.CosmosDB;

namespace Raet.UM.HAS.Infrastructure.Integration.Tests
{
    [TestClass]
    class AssemblyInitialization
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ExternalIdProfile>();
                cfg.AddProfile<PermissionProfile>();
                cfg.AddProfile<EffectiveAuthorizationProfile>();
                cfg.AddProfile<EffectiveAuthorizationGrantedEventProfile>();
                cfg.AddProfile<EffectiveAuthorizationRevokedEventProfile>();
            });
        }
    }
}
