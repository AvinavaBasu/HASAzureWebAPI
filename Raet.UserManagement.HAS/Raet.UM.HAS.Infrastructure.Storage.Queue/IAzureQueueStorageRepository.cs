namespace Raet.UM.HAS.Infrastructure.Storage.Queue
{
    public interface IAzureQueueStorageRepository
    {
        void AddAsync(string message);
    }
}
