namespace DSharp.Dlive.Subscription.Chat
{
    public class UserChatMessage : ChatMessage
    {
        public PublicUserData User { get; }
        public RoomRole RoomRole { get; }

        public UserChatMessage(ChatEventType eventType, string channel, string messageId, PublicUserData user, RoomRole roomRole) : base(eventType, channel, messageId)
        {
            User = user;
            RoomRole = roomRole;
        }
    }
}