using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raet.UM.HAS.DTOs;
using Raet.UM.HAS.Client.Http;

namespace Raet.UM.HAS.Client
{
    public static class HasClientFactory
    {
        public static IPushClient CreateApiClient(HttpClientSettings settings)
        {
            return new PushClient(new HttpEventPusher<EffectiveAuthorizationEvent>(settings, new PingFederateAuthenticationProvider(settings.Authority.AbsoluteUri, settings.ClientId, settings.ClientSecret, settings.XClientIdHeader)));
        }
    }
}
