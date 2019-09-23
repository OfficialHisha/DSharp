namespace DSharp.Subscription.Chat
{
    public class ChatTimeoutMessage : ChatMessage
    {
        public int TimeoutMinutes { get; }
        public PublicUserData TimedoutUser { get; }
        public PublicUserData Admin { get; }

        public ChatTimeoutMessage(string id, int duration, PublicUserData timedoutUser, PublicUserData admin) : base(ChatEventType.TIMEOUT, id)
        {
            TimeoutMinutes = duration;
            TimedoutUser = timedoutUser;
            Admin = admin;
        }
    }
}
