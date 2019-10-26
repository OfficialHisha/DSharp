namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatTimeoutMessage : ChatMessage
    {
        public int TimeoutMinutes { get; }
        public PublicUserData TimedoutUser { get; }
        public PublicUserData Admin { get; }
        public RoomRole AdminRoomRole { get; }
        public RoomRole TimedoutRoomRole { get; }

        public ChatTimeoutMessage(string channel, string messageId, int duration, PublicUserData timedoutUser, RoomRole timedoutRoomRole, PublicUserData admin, RoomRole adminRoomRole) : base(ChatEventType.TIMEOUT, channel, messageId)
        {
            TimeoutMinutes = duration;
            TimedoutUser = timedoutUser;
            TimedoutRoomRole = timedoutRoomRole;
            Admin = admin;
            AdminRoomRole = adminRoomRole;
        }
    }
}
