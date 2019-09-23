namespace DSharp.Subscription.Chat
{
    public class ChatBanMessage : ChatMessage
    {
        public PublicUserData BannedUser { get; }
        public PublicUserData Admin { get; }

        public ChatBanMessage(string id, PublicUserData bannedUser, PublicUserData admin) : base(ChatEventType.BAN, id)
        {
            BannedUser = bannedUser;
            Admin = admin;
        }
    }
}
