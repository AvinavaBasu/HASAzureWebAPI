using Microsoft.Extensions.Logging;
using Raet.UM.HAS.Core.Reporting;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Infrastructure.Http.Common;
using Raet.UM.HAS.Infrastructure.Storage.Table;

namespace Raet.UM.HAS.Infrastructure.Http
{
    public class PersonalInfoExternalServiceFactory : IPersonalInfoExternalServiceFactory
    {
        private readonly IContextMappingLocalStorage ctxMappingLocalStorage;

        private readonly IAuthenticationProvider authenticationProvider;

        private readonly ILogger logger;

        public PersonalInfoExternalServiceFactory(IContextMappingLocalStorage ctxMappingLocalStorage, IAuthenticationProvider authenticationProvider, ILogger logger)
        {
            this.ctxMappingLocalStorage = ctxMappingLocalStorage;
            this.authenticationProvider = authenticationProvider;
            this.logger = logger;
        }

        public IPersonalInfoExternalService Resolve(string context)
        {
            var baseUri = ctxMappingLocalStorage.Resolve(context);
            if (!string.IsNullOrEmpty(baseUri))
                return new PersonalInfoExternalService(baseUri, authenticationProvider, logger);
            
            logger.LogDebug($"PersonalInfoExternalServiceFactory: Error in ContextMapping Table Storage, can't find a valid Url for given context {context}");
            return null;
        }
    }
}
