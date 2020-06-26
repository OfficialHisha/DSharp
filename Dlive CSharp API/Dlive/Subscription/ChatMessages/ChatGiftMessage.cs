using System;

namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatGiftMessage : UserChatMessage
    {
        public GiftType GiftType { get; }
        [Obsolete("Use GiftValue instead")]
        public int GiftLinoValue { get; }
        [Obsolete("Use GiftValue instead")]
        public int GiftLemonValue { get; }
        [Obsolete("For the time being this will always be 1 for lemon donations and the amount of tokens for crypto donations. Because of this, this property has been merged into GiftValue")]
        public int AmountGifts { get; }
        public decimal GiftValue { get; }
        public string GiftMessage { get; }

        public ChatGiftMessage(string channel, string messageId, PublicUserData user, RoomRole roomRole, bool subscribing, GiftType type, int amount, string message) : base(ChatEventType.GIFT, channel, messageId, user, roomRole, subscribing)
        {
            GiftType = type;
            GiftMessage = message;
            AmountGifts = amount;

            switch (type)
            {
                case GiftType.LEMON:
                    GiftValue = 1m;
                    break;
                case GiftType.ICE_CREAM:
                    GiftValue = 10m;
                    break;
                case GiftType.DIAMOND:
                    GiftValue = 100m;
                    break;
                case GiftType.NINJAGHINI:
                    GiftValue = 1000m;
                    break;
                case GiftType.NINJET:
                    GiftValue = 10000m;
                    break;
                case GiftType.BTT:
                case GiftType.TRX:
                    GiftValue = (decimal)amount / 1000000;
                    break;
                default:
                    break;
            }
            GiftLinoValue = GiftLemonValue = (int)GiftValue;
        }
    }
}
