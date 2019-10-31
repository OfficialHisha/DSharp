namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatTextMessage : UserChatMessage
    {
        public string Content { get; }
        public long SubStreak { get; }
        
        public ChatTextMessage(string channel, string messageId, PublicUserData user, RoomRole roomRole, bool subscribing, string content, long subStreak) : base(ChatEventType.MESSAGE, channel, messageId, user, roomRole, subscribing)
        {
            Content = content;
            SubStreak = subStreak;
        }
    }
}
