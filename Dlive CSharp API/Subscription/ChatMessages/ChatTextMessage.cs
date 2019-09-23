namespace DSharp.Subscription.Chat
{
    public class ChatTextMessage : UserChatMessage
    {
        public string Content { get; }
        
        public ChatTextMessage(string id, string content, PublicUserData user) : base(ChatEventType.MESSAGE, id, user)
        {
            Content = content;
        }
    }
}
