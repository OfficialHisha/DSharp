namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatTextMessage : UserChatMessage
    {
        public string Content { get; }
        
        public ChatTextMessage(string channel, string messageId, string content, PublicUserData user) : base(ChatEventType.MESSAGE, channel, messageId, user)
        {
            Content = content;
        }
    }
}
