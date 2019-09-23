namespace DSharp.Subscription.Chat
{
    public class UserChatMessage : ChatMessage
    {
        public PublicUserData User { get; }

        public UserChatMessage(ChatEventType eventType, string channelId, PublicUserData user) : base(eventType, channelId)
        {
            User = user;
        }
    }
}