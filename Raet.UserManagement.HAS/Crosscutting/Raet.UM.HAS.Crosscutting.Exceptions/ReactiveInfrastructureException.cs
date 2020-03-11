using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Raet.UM.HAS.Crosscutting.Exceptions
{
    public class ReactiveInfrastructureException : Exception
    {
        public ReactiveInfrastructureException()
        {
        }

        public ReactiveInfrastructureException(string message) : base(message)
        {
        }

        public ReactiveInfrastructureException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReactiveInfrastructureException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
