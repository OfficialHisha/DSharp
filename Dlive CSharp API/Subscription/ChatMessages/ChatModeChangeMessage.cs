namespace DSharp.Subscription.Chat
{
    public class ChatModeChangeMessage : UserChatMessage
    {
        public ChatMode NewMode { get; }
        
        public ChatModeChangeMessage(string id, ChatMode mode, PublicUserData user) : base(ChatEventType.CHAT_MODE, id, user)
        {
            NewMode = mode;
        }
    }
}
