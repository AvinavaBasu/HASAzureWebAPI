using AutoMapper;
using System;
using Domain = Raet.UM.HAS.Core.Domain;
using Models = Raet.UM.HAS.Infrastructure.Storage.Models;

namespace Raet.UM.HAS.Infrastructure.Storage.CosmosDB
{
    public static class DomainAdapter
    {
        public static Models.WriteEffectiveAuthorizationEvent MapDomainToWriteStorageModel(Domain.EffectiveAuthorizationEvent effectiveAuthorizationEvent)
        {
            var eventType = effectiveAuthorizationEvent.GetType();
            if (eventType == typeof(Domain.EffectiveAuthorizationGrantedEvent))
            {
                return Mapper.Map<Models.WriteEffectiveAuthorizationGrantedEvent>(effectiveAuthorizationEvent);
            }
            else if (eventType == typeof(Domain.EffectiveAuthorizationRevokedEvent))
            {
                return Mapper.Map<Models.WriteEffectiveAuthorizationRevokedEvent>(effectiveAuthorizationEvent);
            }

            throw new NotImplementedException("Unrecognized Effective Authorisation Event");
        }

        public static Domain.EffectiveAuthorizationEvent MapReadStorageModelToDomain(Models.ReadEffectiveAuthorizationEvent readModelEvent)
        {
            switch (readModelEvent.Action)
            {
                case "granted":
                    return Mapper.Map<Domain.EffectiveAuthorizationGrantedEvent>(readModelEvent);

                case "revoked":
                    return Mapper.Map<Domain.EffectiveAuthorizationRevokedEvent>(readModelEvent);

                default:
                    throw new NotImplementedException("Unrecognized Effective Authorisation Event");
            }
        }
    }
}
