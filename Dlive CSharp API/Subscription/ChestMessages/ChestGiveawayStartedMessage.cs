using System;

namespace DSharp.Chest
{
    public class ChestGiveawayStartedMessage : ChestMessage
    {
        public float GiveawayAmount { get; }
        public int DurationSeconds { get; }
        public DateTime GiveawayDeadline { get; }

        public ChestGiveawayStartedMessage(string id, float amount, int duration, DateTime deadline) : base(ChestEventType.GIVEAWAYSTARTED, id)
        {
            GiveawayAmount = amount;
            DurationSeconds = duration;
            GiveawayDeadline = deadline;
        }
    }
}
