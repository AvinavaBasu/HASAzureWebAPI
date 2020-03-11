using System;

namespace Raet.UM.HAS.Crosscutting.Exceptions
{
    public class InvalidEventDataException : Exception
    {
        public InvalidEventDataException()
        {
        }

        public InvalidEventDataException(string message) : base(message)
        {
        }

        public InvalidEventDataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
