namespace DSharp.Chat
{
    public class ChatBanMessage : ChatMessage
    {
        public object User { get; }
        public object Admin { get; }

        public ChatBanMessage(string id, object user, object admin) : base(ChatEventType.BAN, id)
        {
            User = user;
            Admin = admin;
        }
    }
}
