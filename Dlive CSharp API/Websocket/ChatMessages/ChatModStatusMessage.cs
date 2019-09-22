namespace DSharp.Chat
{
    public class ChatModStatusChangeMessage : ChatMessage
    {
        public ModeratorStatusChange StatusChange { get; }
        public object User { get; }
        
        public ChatModStatusChangeMessage(string id, ModeratorStatusChange change, object user) : base(ChatEventType.MOD, id)
        {
            StatusChange = change;
            User = user;
        }
    }
}
