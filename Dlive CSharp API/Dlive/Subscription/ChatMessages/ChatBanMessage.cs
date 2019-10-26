namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatBanMessage : ChatMessage
    {
        public PublicUserData BannedUser { get; }
        public PublicUserData Admin { get; }
        public RoomRole BannedRoomRole { get; set; }
        public RoomRole AdminRoomRole { get; set; }

        public ChatBanMessage(string channel, string messageId, PublicUserData bannedUser, RoomRole bannedRoomRole, PublicUserData admin, RoomRole adminRoomRole) : base(ChatEventType.BAN, channel, messageId)
        {
            BannedUser = bannedUser;
            BannedRoomRole = bannedRoomRole;
            Admin = admin;
            AdminRoomRole = adminRoomRole;
        }
    }
}
