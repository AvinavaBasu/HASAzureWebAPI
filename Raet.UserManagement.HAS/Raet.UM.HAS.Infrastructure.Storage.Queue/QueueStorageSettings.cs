using Raet.UM.HAS.Infrastructure.Storage.Queue;

namespace Raet.UM.HAS.Infrastructure.Storage.Queue
{
    public class QueueStorageSettings : IQueueStorageSettings
    {
        public string ConnectionString { get; set; }

        public string ContainerName { get; set; }
    }
}
