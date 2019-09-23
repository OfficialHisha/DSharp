namespace DSharp.Subscription.Chat
{
    public class ChatSubscriptionMessage : UserChatMessage
    {
        public int Months { get; }
        
        public ChatSubscriptionMessage(string id, int months, PublicUserData user) : base(ChatEventType.SUBSCRIPTION, id, user)
        {
            Months = months;
        }
    }
}
