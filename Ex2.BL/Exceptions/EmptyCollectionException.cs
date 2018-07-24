using System;
using System.Runtime.Serialization;

namespace Ex2.BL.Exceptions
{
    /// <summary>
    ///     Exception witch need to throw when collection is empty
    /// </summary>
    public class EmptyCollectionException : Exception
    {
        public EmptyCollectionException()
        {
        }

        public EmptyCollectionException(string message)
            : base(message)
        {
        }

        public EmptyCollectionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected EmptyCollectionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}