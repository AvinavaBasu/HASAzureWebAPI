using Raet.UM.HAS.Infrastructure.Storage.Table.Model;

namespace Raet.UM.HAS.Infrastructure.Storage.Table
{
    public class ContextMappingLocalStorage : IContextMappingLocalStorage
    {
        IAzureTableStorageRepository<ContextMapping> _client;

        public ContextMappingLocalStorage( IAzureTableStorageRepository<ContextMapping> client) {
            _client = client;
        }
        public string Resolve(string context)
        {
            var res = _client.RetrieveRecord("LocalUserInformation", context).Result;
            return res != null? res.URL:null;
        }
    }
}
