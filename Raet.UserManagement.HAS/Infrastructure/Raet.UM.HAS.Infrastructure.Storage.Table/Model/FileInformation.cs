using Microsoft.WindowsAzure.Storage.Table;
using Raet.UM.HAS.Core.Domain;
using System;

namespace Raet.UM.HAS.Infrastructure.Storage.Table.Model
{
    public class FileInformation : TableEntity
    {
        public string FileName { get; set; }
        public string FileNameTimeStamp { get; set; }
        public string Url { get; set; }
        public string Hash { get; set; }
        public string Status { get; set; }
        public static FileInformation Get(ReportingEvent reportingEvent, Uri url, string hash, string status)
        {
            return new FileInformation
            {
                FileName = reportingEvent.FileName,
                FileNameTimeStamp = reportingEvent.FileNameTimeStamp,
                Url = url.ToString(),
                PartitionKey = reportingEvent.TenantId,
                RowKey =  reportingEvent.Guid,
                Hash = hash,
                Status = status
            };
        }
    }
}
