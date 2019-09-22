using System;

namespace DSharp.Chest
{
    public class ChestValueExpiredMessage : ChestMessage
    {
        public float NewChestValue { get; }
        public DateTime ExpirationDate { get; }

        public ChestValueExpiredMessage(string id, float value, DateTime expirationDate) : base(ChestEventType.VALUEEXPIRED, id)
        {
            NewChestValue = value;
            ExpirationDate = expirationDate;
        }
    }
}
