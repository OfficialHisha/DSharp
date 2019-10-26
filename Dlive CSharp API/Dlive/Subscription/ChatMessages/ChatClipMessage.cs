using System;

namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatClipMessage : UserChatMessage
    {
        public Uri ClipUri { get; }

        public ChatClipMessage(string channel, string messageId, Uri link, PublicUserData user, RoomRole roomRole) : base(ChatEventType.CLIP, channel, messageId, user, roomRole)
        {
            ClipUri = link;
        }
    }
}
