namespace DSharp.Chat
{
    public class ChatFollowMessage : ChatMessage
    {
        public object User { get; }
        
        public ChatFollowMessage(string id, object user) : base(ChatEventType.FOLLOW, id)
        {
            User = user;
        }
    }
}
