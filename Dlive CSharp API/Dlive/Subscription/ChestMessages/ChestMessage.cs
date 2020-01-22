namespace DSharp.Dlive.Subscription.Chest
{
    public class ChestMessage
    {
        public string SubscriptionId { get; }
        public string ChannelId { get; }
        public ChestEventType Type { get; }

        public ChestMessage(ChestEventType type, string id)
        {
            SubscriptionId = id;
            ChannelId = id.Split("_")[0];
            Type = type;
        }
    }
}
