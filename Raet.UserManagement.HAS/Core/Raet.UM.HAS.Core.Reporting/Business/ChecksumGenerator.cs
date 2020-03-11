using Raet.UM.HAS.Core.Reporting.Interface;
using System;
using System.Text;

namespace Raet.UM.HAS.Core.Reporting.Business
{
    public class ChecksumGenerator : IChecksumGenerator
    {
        public string Generate(string file)
        {
            using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(file)));
            }
        }
    }
}
