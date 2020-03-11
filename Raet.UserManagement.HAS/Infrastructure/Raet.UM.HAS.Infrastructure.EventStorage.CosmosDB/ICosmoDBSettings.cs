namespace Raet.UM.HAS.Infrastructure.Storage.CosmosDB
{
    public interface ICosmoDBSettings
    {
        string AuthKey { get; set; }
        string Collection { get; set; }
        string Database { get; set; }
        string Endpoint { get; set; }
    }
}