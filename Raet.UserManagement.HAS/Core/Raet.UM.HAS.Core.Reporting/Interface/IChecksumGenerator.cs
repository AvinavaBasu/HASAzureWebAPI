using System;
using System.Collections.Generic;
using System.Text;

namespace Raet.UM.HAS.Core.Reporting.Interface
{
    public interface IChecksumGenerator
    {
        string Generate(string file);
    }
}
