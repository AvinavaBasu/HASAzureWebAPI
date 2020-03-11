using Microsoft.Extensions.Logging;

namespace Raet.UM.HAS.Core.Reporting.Interface
{
    public interface IDataEnrichmentBusiness
    {
        void Process(string eventData);
    }
}
