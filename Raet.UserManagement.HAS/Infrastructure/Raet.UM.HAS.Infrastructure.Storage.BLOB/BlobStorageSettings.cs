namespace Raet.UM.HAS.Infrastructure.Storage.Blob
{
    public class BlobStorageSettings : IBLOBStorageSettings
    {
        public string ConnectionString { get; set; }

        public string ContainerName { get; set; }
        public string MaxDataLimitForBlob { get; set; }
    }
}
