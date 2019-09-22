namespace DSharp.Chat
{
    public class ChatEmoteMessage : ChatMessage
    {
        public string Emote { get; }
        public object User { get; }
        
        public ChatEmoteMessage(string id, string emote, object user) : base(ChatEventType.EMOTE, id)
        {
            Emote = emote;
            User = user;
        }
    }
}
