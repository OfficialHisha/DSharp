namespace DSharp.Chat
{
    public class ChatTimeoutMessage : ChatMessage
    {
        public int TimeoutMinutes { get; }
        public object User { get; }
        public object Admin { get; }

        public ChatTimeoutMessage(string id, int duration, object user, object admin) : base(ChatEventType.TIMEOUT, id)
        {
            TimeoutMinutes = duration;
            User = user;
            Admin = admin;
        }
    }
}
