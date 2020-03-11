namespace Raet.UM.HAS.Infrastructure.Integration.Tests
{
    public static class InfrastructureConfiguration
    {
        public const string TableStorageConnString = "DefaultEndpointsProtocol=https;AccountName=raetgdprtbldev;AccountKey=qqCd1mOJcAU151rMmVtlfEOCthcFV9ae8q3MKYsqj/Cl6sqBXbKHldeSN8FaNCCunHZ17b3TrLObrPhJxSujhA==;EndpointSuffix=core.windows.net";

        public const string PortalExternalResolveUri = "http://we-d-app-youforce-ext-resolver.azurewebsites.net/api/persons/{id}/personaldetails";

        public const string AuthProviderUri = "https://raet-ib-test.westeurope.cloudapp.azure.com:9031/as/token.oauth2";

        public const string AuthProviderClient = "HAS_system";

        public const string AuthProviderSecret = "1A158BgyFAAAMATWJhNGuJSVD1aOEZ9ZklLQtg2rik2sVU0Lzju1uhvbg3aXnQBW";

        #region CosmoDBSettings
        public const string CosmoDBSettingsDatabase = "EAEventStore";

        public const string CosmoDBSettingsCollection = "EAEvents";

        public const string CosmoDBSettingsEndpoint = "https://raet-gdpr-dev.documents.azure.com:443/";

        public const string CosmoDBSettingsAuthKey =  "Df9lY5JRiVNT7c5JzIag0BNtNxvH6zscFidGkhmp7B4i6dksh27jxs9eAjxHKblQ4fnuXOvuhAxzyVC90cxTZw==";
        #endregion
    }
}
