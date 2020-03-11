namespace Raet.UM.HAS.DTOs
{
    public static class ExtensionMethods
    {
        private const string CompaniesContext = "YOUFORCE.Companies";

        public static void AddTargetCompany(this EffectiveAuthorization effectiveAuthorization, string companyId)
        {
            ExternalId company = new ExternalId(CompaniesContext, companyId);

            effectiveAuthorization.Target = company;
        }
    }
}
