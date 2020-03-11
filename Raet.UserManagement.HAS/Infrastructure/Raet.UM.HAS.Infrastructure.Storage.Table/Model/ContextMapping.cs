using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace Raet.UM.HAS.Infrastructure.Storage.Table.Model
{
    public class ContextMapping: TableEntity
    {
        public String URL { get; set; }

        public ContextMapping()
        {
        
        }

    }
}
