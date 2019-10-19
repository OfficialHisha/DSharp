namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatModStatusChangeMessage : UserChatMessage
    {
        public ModeratorStatusChange StatusChange { get; }
        
        public ChatModStatusChangeMessage(string channel, string messageId, ModeratorStatusChange change, PublicUserData user) : base(ChatEventType.MOD, channel, messageId, user)
        {
            StatusChange = change;
        }
    }
}
