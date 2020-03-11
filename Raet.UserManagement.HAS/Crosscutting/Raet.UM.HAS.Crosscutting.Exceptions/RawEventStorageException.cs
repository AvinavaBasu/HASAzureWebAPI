using System;
using System.Runtime.Serialization;

namespace Raet.UM.HAS.Crosscutting.Exceptions
{
    [Serializable]
    public class RawEventStorageException : Exception
    {
        public RawEventStorageException()
        {
        }

        public RawEventStorageException(string message) : base(message)
        {
        }

        public RawEventStorageException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RawEventStorageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}