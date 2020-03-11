using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raet.UM.HAS.Infrastructure.Storage.Table;
using Raet.UM.HAS.Infrastructure.Storage.Table.Model;
using Raet.UM.HAS.Mocks;
using System;

namespace Raet.UM.HAS.Infrastructure.Tests.TableStorage
{
    [TestClass]
    public class AzureTableStorageClientTests
    {
        readonly MockInMemoryLogger logger = new MockInMemoryLogger();
        private TableStorageSettings settings;
        private AzureTableStorageInitializer initializer;

        [TestInitialize]
        public void BeforeTests() {

            settings = new TableStorageSettings()
            {
                ConnectionString = "DefaultEndpointsProtocol=https;AccountName=raetgdprtbldev;AccountKey=qqCd1mOJcAU151rMmVtlfEOCthcFV9ae8q3MKYsqj/Cl6sqBXbKHldeSN8FaNCCunHZ17b3TrLObrPhJxSujhA==;EndpointSuffix=core.windows.net"
            };

            initializer = new AzureTableStorageInitializer(settings);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OnInitializationWhenNullSettingsProvidedArgumentNullExceptionIsThrown()
        {
            var sut= new AzureTableStorageRepository<ContextMapping>(null, logger);
        }

        [TestMethod]
        public void OnInitializationWhenValidSettingsProvidedAnInstanceIsReturned()
        {
            var sut = new AzureTableStorageRepository<ContextMapping>(initializer, logger);
            Assert.IsNotNull(sut);
            Assert.IsInstanceOfType(sut, typeof(AzureTableStorageRepository<ContextMapping>));
        }
    }
}
