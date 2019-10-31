using System;

namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatGiftMessage : UserChatMessage
    {
        public GiftType GiftType { get; }
        [Obsolete("Use GiftLemonValue instead")]
        public int GiftLinoValue { get; }
        public int GiftLemonValue { get; }
        public int AmountGifts { get; }
        public string GiftMessage { get; }

        public ChatGiftMessage(string channel, string messageId, PublicUserData user, RoomRole roomRole, bool subscribing, GiftType type, int amount, string message) : base(ChatEventType.GIFT, channel, messageId, user, roomRole, subscribing)
        {
            GiftType = type;
            GiftMessage = message;
            AmountGifts = amount;

            switch (type)
            {
                case GiftType.LEMON:
                    GiftLemonValue = 1;
                    break;
                case GiftType.ICE_CREAM:
                    GiftLemonValue = 10;
                    break;
                case GiftType.DIAMOND:
                    GiftLemonValue = 100;
                    break;
                case GiftType.NINJAGHINI:
                    GiftLemonValue = 1000;
                    break;
                case GiftType.NINJET:
                    GiftLemonValue = 10000;
                    break;
                default:
                    break;
            }
            GiftLemonValue *= amount;
            GiftLinoValue = GiftLemonValue;
        }
    }
}
