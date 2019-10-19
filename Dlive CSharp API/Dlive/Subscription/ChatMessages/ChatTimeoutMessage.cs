namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatTimeoutMessage : ChatMessage
    {
        public int TimeoutMinutes { get; }
        public PublicUserData TimedoutUser { get; }
        public PublicUserData Admin { get; }

        public ChatTimeoutMessage(string channel, string messageId, int duration, PublicUserData timedoutUser, PublicUserData admin) : base(ChatEventType.TIMEOUT, channel, messageId)
        {
            TimeoutMinutes = duration;
            TimedoutUser = timedoutUser;
            Admin = admin;
        }
    }
}
