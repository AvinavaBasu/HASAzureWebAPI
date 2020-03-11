using System.IO;

namespace Raet.UM.HAS.DTOs
{
    public class DownloadReport
    {
        public string FileName { get; set; }
        public Stream Stream { get; set; }
    }
}
