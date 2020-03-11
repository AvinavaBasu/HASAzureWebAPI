using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Client.Http
{
    public interface IAuthenticationProvider
    {
        string GetJwt();

        Task<string> GetJwtAsync();
    }
}
