namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatMessage
    {
        public ChatEventType EventType { get; }
        public string ChannelId { get; }
        public string SubscriptionId { get; }
        public string MessageId { get; }

        public ChatMessage(ChatEventType eventType, string channel, string messageId)
        {
            EventType = eventType;
            SubscriptionId = channel;
            ChannelId = channel.Split("_")[0];
            MessageId = messageId;
        }
    }
}
