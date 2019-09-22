﻿namespace DSharp.Chest
{
    public class ChestMessage
    {
        public string ChannelId { get; }
        public ChestEventType Type { get; }

        public ChestMessage(ChestEventType type, string id)
        {
            ChannelId = id;
            Type = type;
        }
    }
}
