using System;
using System.Runtime.Serialization;

namespace Ex2.BL.Exceptions
{
    public class InvalidMailExeption : Exception
    {
        public InvalidMailExeption()
        {
        }

        public InvalidMailExeption(string message)
            : base(message)
        {
        }

        public InvalidMailExeption(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidMailExeption(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}