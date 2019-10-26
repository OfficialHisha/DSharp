namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatSubscriptionMessage : UserChatMessage
    {
        public int Months { get; }
        
        public ChatSubscriptionMessage(string channel, string messageId, int months, PublicUserData user, RoomRole roomRole) : base(ChatEventType.SUBSCRIPTION, channel, messageId, user, roomRole)
        {
            Months = months;
        }
    }
}
