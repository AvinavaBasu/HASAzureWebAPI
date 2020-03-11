using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raet.UM.HAS.Client.Interfaces
{
    public interface IEventPushResponse
    {
        bool IsSuccess { get; }

        string Message { get; }
    }
}
