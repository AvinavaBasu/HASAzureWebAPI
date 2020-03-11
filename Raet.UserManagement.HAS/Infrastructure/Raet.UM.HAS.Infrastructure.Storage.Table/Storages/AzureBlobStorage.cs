//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Raet.UM.HAS.Infrastructure.Storage.Table.Storages
//{
//    public class AzureBlobStorage
//    {
//        private static BlobStorageSettings _configuration;


//        public AzureBlobStorage(IOptions<BlobStorageSettings> config)
//        {
//            _configuration = config.Value;
//        }

//        public static AzureBlobStorageRepository GetInstance(ILogger log)
//        {

//            var initializer = new AzureBlobStorageInitializer(_configuration, log);
//            var azureStorageRepository = new AzureBlobStorageRepository(initializer, _configuration.ContainerName, log);
//            return azureStorageRepository;
//        }
//    }
//}
