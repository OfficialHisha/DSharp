namespace DSharp.Subscription.Chat
{
    public class ChatGiftMessage : UserChatMessage
    {
        public GiftType GiftType { get; }
        public int GiftLinoValue { get; }
        public int AmountGifts { get; }
        public string GiftMessage { get; }

        public ChatGiftMessage(string id, GiftType type, int amount, string message, PublicUserData user) : base(ChatEventType.GIFT, id, user)
        {
            GiftType = type;
            GiftMessage = message;
            AmountGifts = amount;

            switch (type)
            {
                case GiftType.LEMON:
                    GiftLinoValue = 1;
                    break;
                case GiftType.ICE_CREAM:
                    GiftLinoValue = 10;
                    break;
                case GiftType.DIAMOND:
                    GiftLinoValue = 100;
                    break;
                case GiftType.NINJAGHINI:
                    GiftLinoValue = 1000;
                    break;
                case GiftType.NINJET:
                    GiftLinoValue = 10000;
                    break;
                default:
                    break;
            }
            GiftLinoValue *= amount;
        }
    }
}
