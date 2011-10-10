using System;
using System.Runtime.Serialization;

namespace XeroApi.OAuth.Framework
{
    public class MissingTokenException : Exception
    {
        public MissingTokenException()
        {
        }

        public MissingTokenException(string message)
            : base(message)
        {
        }

        public MissingTokenException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public MissingTokenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}
