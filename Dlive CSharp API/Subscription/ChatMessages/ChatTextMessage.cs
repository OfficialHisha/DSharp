namespace DSharp.Chat
{
    public class ChatTextMessage : ChatMessage
    {
        public string Content { get; }
        public object User { get; }

        //User stuff
        string Displayname { get; }
        string Username { get; }
        string AvatarUri { get; }
        PartnerStatus PartnerStatus { get; }
        bool Subscriber { get; }
        RoomRole RoomRole { get; }
        GlobalRole GlobalRole { get; }
        
        public ChatTextMessage(string id, string content, object user) : base(ChatEventType.MESSAGE, id)
        {
            Content = content;
            User = user;
        }
    }
}
