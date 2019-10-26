namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatTextMessage : UserChatMessage
    {
        public string Content { get; }
        
        public ChatTextMessage(string channel, string messageId, string content, PublicUserData user, RoomRole roomRole) : base(ChatEventType.MESSAGE, channel, messageId, user, roomRole)
        {
            Content = content;
        }
    }
}
