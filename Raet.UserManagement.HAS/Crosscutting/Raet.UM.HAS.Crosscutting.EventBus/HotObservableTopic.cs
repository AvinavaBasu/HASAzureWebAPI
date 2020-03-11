using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Crosscutting.EventBus
{
    public class HotObservableTopic<T> : ITopic<T>,IDisposable
    {
        private readonly Subject<T> _subject = new Subject<T>();

        public Task DispatchAsync(T payload)
        {
            _subject.OnNext(payload);
            return Task.CompletedTask;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _subject.Subscribe(observer);
        }

        public void Dispose()
        {
            _subject.Dispose();
        }
    }
}
