using System;
using System.Runtime.Serialization;

namespace Ex2.BL.Exceptions
{
    public class SmptClientException : Exception
    {
        public SmptClientException()
        {
        }

        public SmptClientException(string message)
            : base(message)
        {
        }

        public SmptClientException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected SmptClientException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}