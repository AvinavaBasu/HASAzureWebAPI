namespace Raet.UM.HAS.Infrastructure.Storage.Models
{
    public class EffectiveAuthorization
    {
        public string TenantId { get; set; }

        public ExternalId User { get; set; }

        public Permission Permission { get; set; }

        public ExternalId Target { get; set; }
    }
}
