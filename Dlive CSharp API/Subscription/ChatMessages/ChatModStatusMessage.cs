namespace DSharp.Subscription.Chat
{
    public class ChatModStatusChangeMessage : UserChatMessage
    {
        public ModeratorStatusChange StatusChange { get; }
        
        public ChatModStatusChangeMessage(string id, ModeratorStatusChange change, PublicUserData user) : base(ChatEventType.MOD, id, user)
        {
            StatusChange = change;
        }
    }
}
