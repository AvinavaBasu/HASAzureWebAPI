using System;
using System.Collections.Generic;
using Raet.UM.HAS.Core.Reporting.Interface;

namespace Raet.UM.HAS.Core.Reporting
{
    public class EffectiveAuthorizationHandlerFactory : IEffectiveAuthorizationHandlerFactory
    {
        private readonly Dictionary<Type, IEventHandler> eventHandlers = new Dictionary<Type, IEventHandler>();

        public IEventHandler GetHandler(object effectiveAuthorizationEvent)
        {
            if (!eventHandlers.TryGetValue(effectiveAuthorizationEvent.GetType(), out IEventHandler handler))
            {
                throw new InvalidOperationException("Unknown event type");
            }
            return handler;
        }

        public void RegisterHandler(Type evenType, IEventHandler eventHandler)
        {
            eventHandlers.Add(evenType, eventHandler);
        }
    }
}