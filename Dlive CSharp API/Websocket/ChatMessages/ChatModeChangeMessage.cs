namespace DSharp.Chat
{
    public class ChatModeChangeMessage : ChatMessage
    {
        public ChatMode NewMode { get; }
        public object User { get; }
        
        public ChatModeChangeMessage(string id, ChatMode mode, object user) : base(ChatEventType.CHAT_MODE, id)
        {
            NewMode = mode;
            User = user;
        }
    }
}
