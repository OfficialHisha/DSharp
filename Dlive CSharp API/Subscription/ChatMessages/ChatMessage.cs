namespace DSharp.Subscription.Chat
{
    public class ChatMessage
    {
        public string ChannelId { get; }
        public ChatEventType EventType { get; }

        public ChatMessage(ChatEventType eventType, string id)
        {
            ChannelId = id;
            EventType = eventType;
        }
    }
}
