namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatModStatusChangeMessage : UserChatMessage
    {
        public ModeratorStatusChange StatusChange { get; }
        
        public ChatModStatusChangeMessage(string channel, string messageId, PublicUserData user, RoomRole roomRole, bool subscribing, ModeratorStatusChange change) : base(ChatEventType.MOD, channel, messageId, user, roomRole, subscribing)
        {
            StatusChange = change;
        }
    }
}
