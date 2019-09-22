namespace DSharp.Chat
{
    public class ChatSubscriptionMessage : ChatMessage
    {
        public int Months { get; }
        public object User { get; }
        
        public ChatSubscriptionMessage(string id, int months, object user) : base(ChatEventType.SUBSCRIPTION, id)
        {
            Months = months;
            User = user;
        }
    }
}
