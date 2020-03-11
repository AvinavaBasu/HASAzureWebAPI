using Raet.UM.HAS.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Client.InitialLoad.FileGenerator
{
    public class Authorization
    {
        public DateTime From { get; set; }

        public EffectiveAuthorization EffectiveAuthorization { get; set; }
    }
}
