using System;

namespace DSharp
{
    class WebSocketConnectionRefusedException : Exception
    {
        public WebSocketConnectionRefusedException(string message) : base(message){}
    }
}
