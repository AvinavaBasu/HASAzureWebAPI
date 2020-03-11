using System;
using System.Collections.Generic;
using System.Text;

namespace Raet.UM.HAS.Infrastructure.Storage.CosmosDB
{
    public class CosmoDBSettings : ICosmoDBSettings
    {
        public string Database{get;set;}
        public string Collection { get; set; }
        public string Endpoint { get; set; }
        public string AuthKey { get; set; }
    }
}
