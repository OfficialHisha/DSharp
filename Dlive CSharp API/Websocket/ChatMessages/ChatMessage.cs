namespace DSharp.Chat
{
    public class ChatMessage
    {
        public string ChannelId { get; }
        public ChatEventType Type { get; }

        public ChatMessage(ChatEventType type, string id)
        {
            ChannelId = id;
            Type = type;
        }
    }
}
