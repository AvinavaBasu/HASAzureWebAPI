using Polly;
using Polly.Fallback;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace Raet.UM.HAS.Crosscutting.EventBus.EventGrid
{
    public class ResilienceHelper
    {
        public static RetryPolicy SetRetryPolicy(int maxRetryAttempts, int pauseBetweenRetrysInSeconds)
        {
            return Policy.Handle<HttpRequestException>()
                         .Or<Microsoft.Rest.Azure.CloudException>()
                    .WaitAndRetryAsync(maxRetryAttempts, i => TimeSpan.FromSeconds(pauseBetweenRetrysInSeconds));
        }

        public static FallbackPolicy<HttpResponseMessage> SetFallBackPolicy()
        {
            return Policy<HttpResponseMessage>.Handle<HttpRequestException>()
                                              .Or<Microsoft.Rest.Azure.CloudException>()
                                              .FallbackAsync(fallbackValue: null,
                                                        onFallbackAsync: async (exception, context) =>
                                                        {
                                                            Trace.TraceError($"Failed  after 3 retrys on EventGrid pushing due to: {exception.Exception.Message}");
                                                        }
                    );
        }
    }
}
