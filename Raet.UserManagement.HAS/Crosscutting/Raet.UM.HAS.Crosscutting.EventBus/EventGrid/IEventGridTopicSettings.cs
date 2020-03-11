namespace Raet.UM.HAS.Crosscutting.EventBus.EventGrid
{
    public interface IEventGridTopicSettings
    {
        string SasKey { get; set; }
        string TopicEndpoint { get; set; }

        bool IsValid { get; }
    }
}