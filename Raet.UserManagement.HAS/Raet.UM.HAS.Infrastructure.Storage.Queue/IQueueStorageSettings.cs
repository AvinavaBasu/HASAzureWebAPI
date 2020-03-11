namespace Raet.UM.HAS.Infrastructure.Storage.Queue
{
    public interface IQueueStorageSettings
    {
        string ConnectionString { get; set; }

        string ContainerName { get; set; }
    }
}