using Raet.UM.HAS.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Client.InitialLoad.FileReader
{
    public interface IInitialLoadFileReader
    {
        IList<EffectiveAuthorizationGrantedEvent> ReadInitialLoadFile(string path);
    }
}
