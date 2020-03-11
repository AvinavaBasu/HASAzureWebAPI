using System;
using System.Collections.Generic;
using System.Text;

namespace Raet.UM.HAS.Infrastructure.Http.Common
{
    public class AuthenticationSettings
    {
        public string RequestUrl { get; private set; }

        public string ClientId { get; private set; }

        public string ClientSecret { get; private set; }

        public AuthenticationSettings(string requestUrl, string clientId, string clientSecret)
        {
            RequestUrl = requestUrl;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
    }
}
