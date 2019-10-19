namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatBanMessage : ChatMessage
    {
        public PublicUserData BannedUser { get; }
        public PublicUserData Admin { get; }

        public ChatBanMessage(string channel, string messageId, PublicUserData bannedUser, PublicUserData admin) : base(ChatEventType.BAN, channel, messageId)
        {
            BannedUser = bannedUser;
            Admin = admin;
        }
    }
}
