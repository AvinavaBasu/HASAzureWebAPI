using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raet.UM.HAS.DTOs;
using Raet.UM.HAS.Client.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Raet.UM.HAS.Client
{
    public class PushClient : IPushClient
    {
        readonly IEventPusher<EffectiveAuthorizationEvent> _eventPusher;

        public PushClient(IEventPusher<EffectiveAuthorizationEvent> eventPusher)
        {
            _eventPusher = eventPusher;
        }

        private void ValidateContent(EffectiveAuthorization authorizationToValidate)
        {
            Validator.ValidateObject(authorizationToValidate, new ValidationContext(authorizationToValidate), true);
            Validator.ValidateObject(authorizationToValidate.User, new ValidationContext(authorizationToValidate.User), true);
            Validator.ValidateObject(authorizationToValidate.Permission, new ValidationContext(authorizationToValidate.Permission), true);

            if (authorizationToValidate.Target != null)
            {
                Validator.ValidateObject(authorizationToValidate.Target, new ValidationContext(authorizationToValidate.Target), true);
            }

        }

        public IEventPushResponse PushEffectiveAuthorizationGranted(EffectiveAuthorization authorization, DateTime effectiveDate)
        {
            ValidateContent(authorization);

            var eventToPush = new EffectiveAuthorizationGrantedEvent() { FromDateTime = effectiveDate, EffectiveAuthorization = authorization };

            return _eventPusher.PushGranted(eventToPush);
        }

        public IEventPushResponse PushEffectiveAuthorizationRevoked(EffectiveAuthorization authorization, DateTime effectiveDate)
        {
            ValidateContent(authorization);

            var eventToPush = new EffectiveAuthorizationRevokedEvent() { UntilDateTime = effectiveDate, EffectiveAuthorization = authorization };

            return _eventPusher.PushRevoked(eventToPush);
        }

        public IEventPushResponse PushCompanyWideEffectiveAuthorizationGranted(string targetCompanyId, ExternalId user, Permission permission, DateTime effectiveDate)
        {
            var effectiveAuthorization = new EffectiveAuthorization(targetCompanyId, user, permission);
            effectiveAuthorization.AddTargetCompany(targetCompanyId);

            return PushEffectiveAuthorizationGranted(effectiveAuthorization, effectiveDate);
        }

        public IEventPushResponse PushCompanyWideEffectiveAuthorizationRevoked(string targetCompanyId, ExternalId user, Permission permission, DateTime effectiveDate)
        {
            var effectiveAuthorization = new EffectiveAuthorization(targetCompanyId, user, permission);
            effectiveAuthorization.AddTargetCompany(targetCompanyId);

            return PushEffectiveAuthorizationRevoked(effectiveAuthorization, effectiveDate);
        }

        public async Task<IEventPushResponse> PushEffectiveAuthorizationGrantedAsync(EffectiveAuthorization authorization, DateTime effectiveDate)
        {
            ValidateContent(authorization);

            var eventToPush = new EffectiveAuthorizationGrantedEvent() { FromDateTime = effectiveDate, EffectiveAuthorization = authorization };

            return await _eventPusher.PushGrantedAsync(eventToPush);
        }

        public async Task<IEventPushResponse> PushEffectiveAuthorizationRevokedAsync(EffectiveAuthorization authorization, DateTime effectiveDate)
        {
            ValidateContent(authorization);

            var eventToPush = new EffectiveAuthorizationRevokedEvent() { UntilDateTime = effectiveDate, EffectiveAuthorization = authorization };

            return await _eventPusher.PushRevokedAsync(eventToPush);
        }

        public async Task<IEventPushResponse> PushCompanyWideEffectiveAuthorizationGrantedAsync(string targetCompanyId, ExternalId user, Permission permission, DateTime effectiveDate)
        {
            var effectiveAuthorization = new EffectiveAuthorization(targetCompanyId, user, permission);
            effectiveAuthorization.AddTargetCompany(targetCompanyId);

            return await PushEffectiveAuthorizationGrantedAsync(effectiveAuthorization, effectiveDate);
        }

        public async Task<IEventPushResponse> PushCompanyWideEffectiveAuthorizationRevokedAsync(string targetCompanyId, ExternalId user, Permission permission, DateTime effectiveDate)
        {
            var effectiveAuthorization = new EffectiveAuthorization(targetCompanyId, user, permission);
            effectiveAuthorization.AddTargetCompany(targetCompanyId);

            return await PushEffectiveAuthorizationRevokedAsync(effectiveAuthorization, effectiveDate);
        }
    }
}
