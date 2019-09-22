using System;

namespace DSharp.Chat
{
    public class ChatClipMessage : ChatMessage
    {
        public object User { get; }
        public Uri ClipUri { get; }

        public ChatClipMessage(string id, object user, Uri link) : base(ChatEventType.CLIP, id) 
        {
            User = user;
            ClipUri = link;
        }
    }
}
