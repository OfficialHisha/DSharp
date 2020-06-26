using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace DSharp.Dlive.Exceptions
{
    internal class AccountException : Exception
    {
        public AccountException()
        {
        }

        public AccountException(string message) : base(message)
        {
        }

        public AccountException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AccountException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
