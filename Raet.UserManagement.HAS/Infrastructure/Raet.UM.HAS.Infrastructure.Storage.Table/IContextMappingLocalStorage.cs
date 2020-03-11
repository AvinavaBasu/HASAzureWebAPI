namespace Raet.UM.HAS.Infrastructure.Storage.Table
{
    public interface IContextMappingLocalStorage
    {
        string Resolve(string context);
    }
}
