namespace DSharp.Chat
{
    public class ChatGiftMessage : ChatMessage
    {
        public GiftType GiftType { get; }
        public int GiftLinoValue { get; }
        public int AmountGifts { get; }
        public string GiftMessage { get; }
        public object User { get; }

        public ChatGiftMessage(string id, GiftType type, int amount, string message, object user) : base(ChatEventType.GIFT, id)
        {
            GiftType = type;
            User = user;
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
