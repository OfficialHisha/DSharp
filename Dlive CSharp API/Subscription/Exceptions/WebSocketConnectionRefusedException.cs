using System;

namespace DSharp.Subscription
{
    class WebSocketConnectionRefusedException : Exception
    {
        public WebSocketConnectionRefusedException(string message) : base(message){}
    }
}
