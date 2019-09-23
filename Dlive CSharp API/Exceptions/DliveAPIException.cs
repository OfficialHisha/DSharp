using System;

namespace DSharp
{
    public abstract class DliveAPIException : Exception
    {
        protected DliveAPIException(string message) : base(message) {}
    }
}