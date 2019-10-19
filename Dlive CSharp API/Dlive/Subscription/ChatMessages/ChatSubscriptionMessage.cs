namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatSubscriptionMessage : UserChatMessage
    {
        public int Months { get; }
        
        public ChatSubscriptionMessage(string channel, string messageId, int months, PublicUserData user) : base(ChatEventType.SUBSCRIPTION, channel, messageId, user)
        {
            Months = months;
        }
    }
}
