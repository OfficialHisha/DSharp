namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatEmoteMessage : UserChatMessage
    {
        public string Emote { get; }
        
        public ChatEmoteMessage(string channel, string messageId, PublicUserData user, RoomRole roomRole, bool subscribing, string emote) : base(ChatEventType.EMOTE, channel, messageId, user, roomRole, subscribing)
        {
            Emote = emote;
        }
    }
}
