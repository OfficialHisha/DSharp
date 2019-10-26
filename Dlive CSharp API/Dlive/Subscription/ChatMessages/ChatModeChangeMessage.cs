﻿namespace DSharp.Dlive.Subscription.Chat
{
    public class ChatModeChangeMessage : UserChatMessage
    {
        public ChatMode NewMode { get; }
        
        public ChatModeChangeMessage(string channel, string messageId, ChatMode mode, PublicUserData user, RoomRole roomRole) : base(ChatEventType.CHAT_MODE, channel, messageId, user, roomRole)
        {
            NewMode = mode;
        }
    }
}
