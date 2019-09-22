namespace DSharp.Chat
{
    public class ChatHostMessage : ChatMessage
    {
        public int Viewers { get; }
        public object User { get; }
        
        public ChatHostMessage(string id, int viewers, object user) : base(ChatEventType.HOST, id)
        {
            Viewers = viewers;
            User = user;
        }
    }
}
