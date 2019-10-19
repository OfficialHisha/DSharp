namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatEmoteMessage : UserChatMessage
    {
        public string Emote { get; }
        
        public ChatEmoteMessage(string channel, string messageId, string emote, PublicUserData user) : base(ChatEventType.EMOTE, channel, messageId, user)
        {
            Emote = emote;
        }
    }
}
