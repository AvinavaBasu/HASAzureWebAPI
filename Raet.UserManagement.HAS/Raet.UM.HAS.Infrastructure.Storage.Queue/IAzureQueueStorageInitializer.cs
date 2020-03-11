using Microsoft.WindowsAzure.Storage.Queue;

namespace Raet.UM.HAS.Infrastructure.Storage.Queue
{
    public interface IAzureQueueStorageInitializer
    {
       CloudQueue Initialize();
    }
}
