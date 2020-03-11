namespace Raet.UM.HAS.Core.Reporting.Interface
{
    public interface IPersonalInfoExternalServiceFactory
    {
        IPersonalInfoExternalService Resolve(string context);
    }
}
