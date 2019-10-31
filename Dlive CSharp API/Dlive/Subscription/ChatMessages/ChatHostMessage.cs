namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatHostMessage : UserChatMessage
    {
        public int Viewers { get; }
        
        public ChatHostMessage(string channel, string messageId, PublicUserData user, RoomRole roomRole, bool subscribing, int viewers) : base(ChatEventType.HOST, channel, messageId, user, roomRole, subscribing)
        {
            Viewers = viewers;
        }
    }
}
