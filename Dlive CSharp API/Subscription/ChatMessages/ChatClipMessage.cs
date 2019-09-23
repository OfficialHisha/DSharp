using System;

namespace DSharp.Subscription.Chat
{
    public class ChatClipMessage : UserChatMessage
    {
        public Uri ClipUri { get; }

        public ChatClipMessage(string id, PublicUserData user, Uri link) : base(ChatEventType.CLIP, id, user) 
        {
            ClipUri = link;
        }
    }
}
