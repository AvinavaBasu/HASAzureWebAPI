using System;
using System.Collections.Generic;

namespace Raet.UM.HAS.Core.Domain
{
    public class ReportingEvent
    {
        public ReportingEvent()
        {
            Guid = System.Guid.NewGuid().ToString();
            TenantId = TenantId;
        }
        public string Guid { get; set; }
        public string Application { get; set; }

        public IList<string> Permissions { get; set; }

        public ExternalId Source { get; set; }

        public ExternalId Target { get; set; }

        public string StartDateFormat
        {
            get { return string.Format("{0:s}", StartDate.ToUniversalTime()); }
        }

        public string EndDateFormat
        {
            get { return string.Format("{0:s}", EndDate?.ToUniversalTime()); }
        }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string FileName { get; set; }
        public string FileNameTimeStamp
        {
            get
            {
                return TenantId + Guid;
            }
        }
        public string TenantId { get; set; }
    }
}
