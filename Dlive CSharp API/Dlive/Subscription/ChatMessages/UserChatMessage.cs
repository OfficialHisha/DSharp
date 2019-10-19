namespace DSharp.Dlive.Subscription.Chat
{
    public class UserChatMessage : ChatMessage
    {
        public PublicUserData User { get; }

        public UserChatMessage(ChatEventType eventType, string channel, string messageId, PublicUserData user) : base(eventType, channel, messageId)
        {
            User = user;
        }
    }
}