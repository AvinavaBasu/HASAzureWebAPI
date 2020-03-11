using Raet.UM.HAS.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Client.InitialLoad.FileGenerator
{
    public interface IInitialLoadFileGenerator
    {
        void GenerateInitialLoadFile(IList<Authorization> authorizations, string outputPath, string tenantId);
    }
}
