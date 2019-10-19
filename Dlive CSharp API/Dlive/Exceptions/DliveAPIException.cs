using System;

namespace DSharp.Dlive
{
    public abstract class DliveAPIException : Exception
    {
        protected DliveAPIException(string message) : base(message) {}
    }
}