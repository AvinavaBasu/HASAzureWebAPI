using System;
using Raet.UM.HAS;
using Raet.UM.HAS.DTOs;

namespace Raet.UM.HAS.Server.WebApi.Adapters
{
    public static class DomainAdapter
    {
        public static Core.Domain.ExternalId MapExternalId(ExternalId dtoExternalId)
        {
            return new Core.Domain.ExternalId() { Id = dtoExternalId.Id.Trim(), Context = dtoExternalId.Context.Trim() };
        }

        public static Core.Domain.Permission MapPermission (Permission dtoPermission)
        {
            return new Core.Domain.Permission()
            {
                Application = dtoPermission.Application.Trim(),
                Id = dtoPermission.Id.Trim(),
                Description = dtoPermission.Description.Trim()
            };
        }

        public static Core.Domain.EffectiveAuthorization MapEffectiveAuthorization(EffectiveAuthorization dtoEffectiveAuthorization)
        {
            var output = new Core.Domain.EffectiveAuthorization()
            {
                TenantId = dtoEffectiveAuthorization.TenantId.Trim(),
                User = MapExternalId(dtoEffectiveAuthorization.User),
                Permission = MapPermission(dtoEffectiveAuthorization.Permission)
            };

            if (dtoEffectiveAuthorization.Target != null)
            {
                output.Target = MapExternalId(dtoEffectiveAuthorization.Target);
            }

            return output;
        }

        public static Core.Domain.EffectiveAuthorizationGrantedEvent MapEvent(EffectiveAuthorizationGrantedEvent dtoEvent)
        {
            return new Core.Domain.EffectiveAuthorizationGrantedEvent()
            {
                EffectiveAuthorization = MapEffectiveAuthorization(dtoEvent.EffectiveAuthorization),
                DateCreated = DateTime.UtcNow,
                From = dtoEvent.FromDateTime.Value
            };
        }

        public static Core.Domain.EffectiveAuthorizationRevokedEvent MapEvent(EffectiveAuthorizationRevokedEvent dtoEvent)
        {
            return new Core.Domain.EffectiveAuthorizationRevokedEvent()
            {
                EffectiveAuthorization = MapEffectiveAuthorization(dtoEvent.EffectiveAuthorization),
                DateCreated = DateTime.UtcNow,
                Until = dtoEvent.UntilDateTime.Value
            };
        }
    }
}
