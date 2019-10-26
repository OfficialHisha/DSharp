namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatGiftSubscriptionMessage : ChatMessage
    {
        public int Months { get; }
        public PublicUserData GiftingUser { get; }
        public PublicUserData ReceivingUser { get; }
        public RoomRole ReceiverRoomRole { get; set; }

        public ChatGiftSubscriptionMessage(string channel, string messageId, int months, PublicUserData giftingUser, PublicUserData receiveingUser, RoomRole receiverRoomRole) : base(ChatEventType.GIFTSUB, channel, messageId)
        {
            Months = months;
            GiftingUser = giftingUser;
            ReceivingUser = receiveingUser;
            ReceiverRoomRole = receiverRoomRole;
        }
    }
}
