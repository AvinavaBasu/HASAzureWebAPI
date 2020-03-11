using Microsoft.Azure.Documents.Client;

namespace Raet.UM.HAS.Infrastructure.Storage.CosmosDB
{
    public interface ICosmoDBStorageInitializer
    {
        ICosmoDBSettings DbConfig { get; }
        DocumentClient Initialize();
    }
}