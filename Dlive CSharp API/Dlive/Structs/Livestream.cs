using System;

namespace DSharp.Dlive
{
    public class Livestream
    {
        public bool XTagged { get; }
        public bool AlertsDisabled { get; }
        public string StreamTitle { get; }
        public string ThumbnailUrl { get; }
        public string Language { get; }
        public string Category { get; }
        public DateTime StartedAt { get; }
        public decimal DonationsReceived { get; }
        public int CurrentConcurrentViewers { get; }
        public int TotalViews { get; set; }
        public PublicUserData Streamer { get; }
    }
}
