using System;
using System.Runtime.Serialization;
namespace Raet.UM.HAS.Crosscutting.Exceptions
{
    public class ReportingStorageException : Exception
    {
        public ReportingStorageException()
        {
        }

        public ReportingStorageException(string message) : base(message)
        {
        }

        public ReportingStorageException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReportingStorageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
