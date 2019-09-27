namespace DSharp.Subscription.Chat
{
    public class ChatModeChangeMessage : UserChatMessage
    {
        public ChatMode NewMode { get; }
        
        public ChatModeChangeMessage(string channel, string messageId, ChatMode mode, PublicUserData user) : base(ChatEventType.CHAT_MODE, channel, messageId, user)
        {
            NewMode = mode;
        }
    }
}
