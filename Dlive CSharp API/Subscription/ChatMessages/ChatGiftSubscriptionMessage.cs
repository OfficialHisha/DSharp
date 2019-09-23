namespace DSharp.Subscription.Chat
{
    public class ChatGiftSubscriptionMessage : ChatMessage
    {
        public int Months { get; }
        public PublicUserData GiftingUser { get; }
        public PublicUserData ReceivingUser { get; }

        public ChatGiftSubscriptionMessage(string id, int months, PublicUserData giftingUser, PublicUserData receiveingUser) : base(ChatEventType.GIFTSUB, id)
        {
            Months = months;
            GiftingUser = giftingUser;
            ReceivingUser = receiveingUser;
        }
    }
}
