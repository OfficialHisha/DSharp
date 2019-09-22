namespace DSharp.Chat
{
    public class ChatGiftSubscriptionMessage : ChatMessage
    {
        public int Months { get; }
        public object GiftingUser { get; }
        public object ReceivingUser { get; }

        public ChatGiftSubscriptionMessage(string id, int months, object giftingUser, object receiveingUser) : base(ChatEventType.GIFTSUB, id)
        {
            Months = months;
            GiftingUser = giftingUser;
            ReceivingUser = receiveingUser;
        }
    }
}
