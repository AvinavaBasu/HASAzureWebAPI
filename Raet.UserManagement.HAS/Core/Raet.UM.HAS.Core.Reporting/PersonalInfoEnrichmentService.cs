using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.Infrastructure.Storage.Table;

namespace Raet.UM.HAS.Core.Reporting
{
    public class PersonalInfoEnrichmentService : IPersonalInfoEnrichmentService
    {
        private readonly IPersonLocalStorage personLocalStorage;

        private readonly IPersonalInfoExternalServiceFactory persInfoExternalServiceFactory;

        private readonly ILogger logger;

        public PersonalInfoEnrichmentService(
            IPersonLocalStorage personLocalStorage, 
            IPersonalInfoExternalServiceFactory persInfoExternalServiceFactory,
            ILogger logger)
        {
            this.personLocalStorage = personLocalStorage;
            this.persInfoExternalServiceFactory = persInfoExternalServiceFactory;
            this.logger = logger;
        }

        /// <summary>
        ///  Creates a Person object based on provided ExternalId 
        /// </summary>
        /// <param name="externalId">Person context identifier</param>
        /// <exception cref="System.ArgumentException">Thrown when ExternalId is not valid (Empty properties)</exception>
        /// <returns>Person enriched with resolved personal information</returns>
        public async Task<Person> ResolvePerson(ExternalId externalId)
        {
            ValidateExternalId(externalId);

            var person = await personLocalStorage.FindPersonAsync(externalId);
            if (person != null)
                return person;

            var persInfoExternalService = persInfoExternalServiceFactory.Resolve(externalId.Context);
            if (persInfoExternalService == null)
            {
                LogResolverApiNotFound(externalId);
                return new Person(externalId, null);
            }
                       
            var personalInfo = await persInfoExternalService.FindPersonalInfoAsync(externalId.Id);
            if (personalInfo == null)
            {
                LogPersonalInfoNotFound(externalId);
                return new Person(externalId, null);
            }
                                    
            return await personLocalStorage.CreatePersonAsync(new Person(externalId, personalInfo));
        }

        private void ValidateExternalId(ExternalId externalId)
        {
            if (externalId == null ||
                string.IsNullOrWhiteSpace(externalId.Context) ||
                string.IsNullOrWhiteSpace(externalId.Id))
            {
                logger.LogError("PersonalInfoEnrichmentService: Invalid External ID");
                throw new System.ArgumentException();
            }
        }

        private void LogResolverApiNotFound(ExternalId externalId)
        {
            string message = $"PersonalInfoEnrichmentService: Error on getting Personal Info for ExternalId [Context: {externalId.Context}, Id: {externalId.Id}]. ";
            message += "Can't find a valid resolver api for given context. ";
            message += "Person will be created based on Technical Keys only.";

            logger.LogWarning(message);
        }

        private void LogPersonalInfoNotFound(ExternalId externalId)
        {
            string message = $"PersonalInfoEnrichmentService: Error on getting Personal Info for ExternalId [Context: {externalId.Context}, Id: {externalId.Id}]. ";
            message += "Personal Info not found by provided resolver api for given context. ";
            message += "Person will be created based on Technical Keys only.";

            logger.LogWarning(message);
        }
    }
}