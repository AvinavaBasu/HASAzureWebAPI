using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Raet.UM.HAS.Client.Http
{
    public class HttpClientSettings
    {
        /// <summary>
        /// The base url of a HAS API
        /// </summary>
        public Uri BaseUrl { get; internal set; }

        /// <summary>
        /// The client application that wants to access the resource
        /// </summary>
        public string ClientId { get; internal set; }

        public string ClientSecret { get; internal set; }

        /// <summary>
        /// Url of the authority that will return the token(s) if authorization is successful
        /// </summary>
        public Uri Authority { get; internal set; }

        public string XClientIdHeader { get; internal set; }

        public HttpClientSettings(Uri urlBase, string clientId, string clientSecret, Uri authority, string xClientIdHeader)
        {
            BaseUrl = urlBase;
            ClientId = clientId;
            ClientSecret = clientSecret;
            Authority = authority;
            XClientIdHeader = xClientIdHeader;
        }
    }
}
