using Raet.UM.HAS.Core.Domain;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Crosscutting.EventBus
{
    public interface IEventBus
    {
        ITopic<T> GetTopic<T>(string name);
    }
}
