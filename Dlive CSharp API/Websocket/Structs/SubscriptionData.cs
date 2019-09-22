namespace DSharp
{
    public struct SubscriptionData
    {
        public string ChatEventId { get; }
        public string ChestEventId { get; }

        public SubscriptionData(string chatEventId = null, string chestEventId = null)
        {
            ChatEventId = chatEventId;
            ChestEventId = chestEventId;
        }
    }
}
