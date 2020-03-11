namespace Raet.UM.HAS.Core.Reporting.Interface
{
    public interface IEffectiveAuthorizationHandlerFactory
    {
        IEventHandler GetHandler(object effectiveAuthorizationEvent);

    }
}