namespace DSharp.Subscription.Chat
{
    public class ChatHostMessage : UserChatMessage
    {
        public int Viewers { get; }
        
        public ChatHostMessage(string channel, string messageId, int viewers, PublicUserData user) : base(ChatEventType.HOST, channel, messageId, user)
        {
            Viewers = viewers;
        }
    }
}
