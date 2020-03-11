using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raet.UM.HAS.Client.Interfaces;

namespace Raet.UM.HAS.Client.Http
{
    public class HttpEventPushResponse: IEventPushResponse
    {
        public bool IsSuccess { get; }

        public string Message { get; }

        public HttpEventPushResponse(bool success, string message)
        {
            IsSuccess = success;
            Message = message;
        }

    }
}
