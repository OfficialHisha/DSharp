namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatModeChangeMessage : UserChatMessage
    {
        public ChatMode NewMode { get; }
        
        public ChatModeChangeMessage(string channel, string messageId, PublicUserData user, RoomRole roomRole, bool subscribing, ChatMode mode) : base(ChatEventType.CHAT_MODE, channel, messageId, user, roomRole, subscribing)
        {
            NewMode = mode;
        }
    }
}
