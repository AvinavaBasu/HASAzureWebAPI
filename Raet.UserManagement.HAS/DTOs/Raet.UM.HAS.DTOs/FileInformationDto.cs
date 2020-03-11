using System;
using System.Collections.Generic;
using System.Text;

namespace Raet.UM.HAS.DTOs
{
    public class FileInformationDto
    {
        public string Guid { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
        public string Hash { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
