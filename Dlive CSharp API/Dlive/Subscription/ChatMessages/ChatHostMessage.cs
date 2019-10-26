namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatHostMessage : UserChatMessage
    {
        public int Viewers { get; }
        
        public ChatHostMessage(string channel, string messageId, int viewers, PublicUserData user, RoomRole roomRole) : base(ChatEventType.HOST, channel, messageId, user, roomRole)
        {
            Viewers = viewers;
        }
    }
}
