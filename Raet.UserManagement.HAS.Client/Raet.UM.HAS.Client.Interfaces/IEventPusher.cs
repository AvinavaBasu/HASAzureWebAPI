using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Client.Interfaces
{
    public interface IEventPusher<TEvent> where TEvent : class
    {
        IEventPushResponse PushGranted(TEvent pushedEvent);

        IEventPushResponse PushRevoked(TEvent pushedEvent);

        Task<IEventPushResponse> PushGrantedAsync(TEvent pushedEvent);

        Task<IEventPushResponse> PushRevokedAsync(TEvent pushedEvent);
    }
}
