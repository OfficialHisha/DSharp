using System;

namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatClipMessage : UserChatMessage
    {
        public Uri ClipUri { get; }

        public ChatClipMessage(string channel, string messageId, PublicUserData user, RoomRole roomRole, bool subscribing, Uri link) : base(ChatEventType.CLIP, channel, messageId, user, roomRole, subscribing)
        {
            ClipUri = link;
        }
    }
}
