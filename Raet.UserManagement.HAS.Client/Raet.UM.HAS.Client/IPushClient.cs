using System;
using System.Threading.Tasks;
using Raet.UM.HAS.DTOs;
using Raet.UM.HAS.Client.Interfaces;

namespace Raet.UM.HAS.Client
{
    public interface IPushClient
    {
        IEventPushResponse PushEffectiveAuthorizationGranted(EffectiveAuthorization authorization, DateTime effectiveDate);

        IEventPushResponse PushEffectiveAuthorizationRevoked(EffectiveAuthorization authorization, DateTime effectiveDate);

        IEventPushResponse PushCompanyWideEffectiveAuthorizationGranted(string targetCompanyId, ExternalId user, Permission permission, DateTime effectiveDate);

        IEventPushResponse PushCompanyWideEffectiveAuthorizationRevoked(string targetCompanyId, ExternalId user, Permission permission, DateTime effectiveDate);

        Task<IEventPushResponse> PushEffectiveAuthorizationGrantedAsync(EffectiveAuthorization authorization, DateTime effectiveDate);

        Task<IEventPushResponse> PushEffectiveAuthorizationRevokedAsync(EffectiveAuthorization authorization, DateTime effectiveDate);

        Task<IEventPushResponse> PushCompanyWideEffectiveAuthorizationGrantedAsync(string targetCompanyId, ExternalId user, Permission permission, DateTime effectiveDate);

        Task<IEventPushResponse> PushCompanyWideEffectiveAuthorizationRevokedAsync(string targetCompanyId, ExternalId user, Permission permission, DateTime effectiveDate);
    }
}