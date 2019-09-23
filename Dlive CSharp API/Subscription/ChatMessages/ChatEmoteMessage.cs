namespace DSharp.Subscription.Chat
{
    public class ChatEmoteMessage : UserChatMessage
    {
        public string Emote { get; }
        
        public ChatEmoteMessage(string id, string emote, PublicUserData user) : base(ChatEventType.EMOTE, id, user)
        {
            Emote = emote;
        }
    }
}
