using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Raet.UM.HAS.Core.Domain;

namespace Raet.UM.HAS.Core.Application
{
    public interface IWriteRawEventStorage
    {
        Task<string> WriteRawEventAsync(EffectiveAuthorizationEvent effectiveAuthorizationEvent);
    }
}
