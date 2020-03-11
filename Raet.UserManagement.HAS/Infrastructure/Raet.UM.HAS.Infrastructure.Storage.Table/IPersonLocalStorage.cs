using System.Threading.Tasks;
using Raet.UM.HAS.Core.Domain;

namespace Raet.UM.HAS.Infrastructure.Storage.Table
{
    public interface IPersonLocalStorage
    {
        Task<Person> FindPersonAsync(ExternalId externalId);

        Task<Person> CreatePersonAsync(Person person);
    }
}
