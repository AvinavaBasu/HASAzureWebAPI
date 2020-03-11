using System.Threading.Tasks;
using Raet.UM.HAS.Core.Domain;

namespace Raet.UM.HAS.Core.Reporting.Interface
{
    public interface IPersonalInfoEnrichmentService
    {
        Task<Person> ResolvePerson(ExternalId externalId);
    }
}