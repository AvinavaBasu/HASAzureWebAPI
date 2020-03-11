namespace Raet.UM.HAS.Infrastructure.Storage.Blob
{
    public interface IBLOBStorageSettings
    {
        string ConnectionString { get; set; }

        string ContainerName { get; set; }
        string MaxDataLimitForBlob { get; set; }
    }
}