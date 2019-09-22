namespace DSharp.Chest
{
    public class ChestValueUpdatedMessage : ChestMessage
    {
        public float NewChestValue { get; }

        public ChestValueUpdatedMessage(string id, float value) : base(ChestEventType.VALUEUPDATED, id)
        {
            NewChestValue = value;
        }
    }
}
