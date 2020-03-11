using System;

namespace Raet.UM.HAS.Infrastructure.Storage.Table
{
    public class TableStorageSettings : ITableStorageSettings
    {
        public string ConnectionString { get; set; }
    }
}