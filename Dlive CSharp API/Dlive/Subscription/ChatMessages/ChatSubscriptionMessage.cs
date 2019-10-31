namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatSubscriptionMessage : UserChatMessage
    {
        public int Months { get; }
        
        public ChatSubscriptionMessage(string channel, string messageId, PublicUserData user, RoomRole roomRole, bool subscription, int months) : base(ChatEventType.SUBSCRIPTION, channel, messageId, user, roomRole, subscription)
        {
            Months = months;
        }
    }
}
