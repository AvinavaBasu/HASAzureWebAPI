using System;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Crosscutting.EventBus
{
    public interface ITopic<T> : IObservable<T>
    {
        Task DispatchAsync(T payload);
    }
}
