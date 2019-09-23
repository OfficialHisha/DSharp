namespace DSharp.Subscription.Chat
{
    public class ChatHostMessage : UserChatMessage
    {
        public int Viewers { get; }
        
        public ChatHostMessage(string id, int viewers, PublicUserData user) : base(ChatEventType.HOST, id, user)
        {
            Viewers = viewers;
        }
    }
}
