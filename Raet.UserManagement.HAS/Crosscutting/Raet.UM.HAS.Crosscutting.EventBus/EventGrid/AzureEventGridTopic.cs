using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Rest.Azure;
using Raet.UM.HAS.Crosscutting.Exceptions;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Crosscutting.EventBus.EventGrid
{
    public class AzureEventGridTopic<T> : ITopic<T>, IDisposable
    {
        private readonly IEventGridTopicSettings _settings;
        private readonly EventGridClient _eventGridClient;
        

        public AzureEventGridTopic(IEventGridTopicSettings settings)
        {
            _settings = settings?? throw new ArgumentNullException(nameof(settings));
            _eventGridClient = new EventGridClient(new TopicCredentials(_settings.SasKey));
        }

        

        /// <summary>
        /// Notifies Azure EventGrid about the ocurence of an event of Topic
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public Task DispatchAsync(T payload)
        {
            try
            {
               return _eventGridClient.PublishEventsWithHttpMessagesAsync( new Uri(_settings.TopicEndpoint).Host,
                                                                           BuildNotifiableEventStructure(payload) );
            }
            catch (CloudException e)
            {
                throw new ReactiveInfrastructureException("Azure EventGrid notification failed, check network availability and configuration", e);
            }

        }

        
        /// <summary>
        /// Build an EventGridEvent List using domain objects data
        /// Note: This is an infrastructure prerequisite to notify Azure EventGrid
        /// </summary>
        /// <param name="authorizationEvent"> Domain event to ve converted and added to notifiable list </param>
        /// <returns> List<EventGridEvent> </returns>
        private List<EventGridEvent> BuildNotifiableEventStructure(T authorizationEvent)
        {

            var _authorizationEvent = authorizationEvent ;

            var eventType = typeof(T).FullName.Split('.').Last();

            var @event = new EventGridEvent()
            {
                Subject = $"Event of type {eventType}",
                EventType = eventType,
                EventTime = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
                Data = _authorizationEvent,
                DataVersion = "1.0"
            };

            return new List<EventGridEvent>(new EventGridEvent[] { @event });
        }

        
        public IDisposable Subscribe(IObserver<T> observer)
        {
            return Disposable.Empty;
        }

        public void Dispose()
        {
            _eventGridClient.Dispose();
        }
    }
}
