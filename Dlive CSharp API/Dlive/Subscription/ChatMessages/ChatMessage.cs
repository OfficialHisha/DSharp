namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatMessage
    {
        public string MessageId { get; }
        public string ChannelId { get; }
        public ChatEventType EventType { get; }

        public ChatMessage(ChatEventType eventType, string channel, string messageId)
        {
            MessageId = messageId;
            ChannelId = channel;
            EventType = eventType;
        }
    }
}
