using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Raet.UM.HAS.Core.Domain;

namespace Raet.UM.HAS.Core.Application
{
    public class WriteStackEffectiveAuthorizationLogging : IEffectiveAuthorizationLogging
    {
        private readonly IWriteRawEventStorage _rawEventStorage;

        public WriteStackEffectiveAuthorizationLogging(IWriteRawEventStorage rawEventStorage)
        {
            _rawEventStorage = rawEventStorage;
        }

        public async Task<string> AddAuthLogAsync(EffectiveAuthorizationEvent effectiveAuthorizationEvent)
        {
            return await _rawEventStorage.WriteRawEventAsync(effectiveAuthorizationEvent);
        }
    }
}
