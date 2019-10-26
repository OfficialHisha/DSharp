using System;

namespace DSharp.Dlive
{
    public struct PublicUserData
    {
        public string Linoname { get; }
        public string Displayname { get; }
        public PartnerStatus PartnerStatus { get; }
        public string Effect { get; set; }
        public bool Deactivated { get; }
        public AboutPanel[] Panels { get; }
        public Uri AvatarUri { get; }
        public long NumFollowers { get; }
        public float ChestValue { get; }
        [Obsolete("Use LemonBalance instead")]
        public float LinoBalance { get; }
        [Obsolete("Use LemonEarnings instead")]
        public float LinoEarnings { get; }
        public float LemonBalance { get; }
        public float LemonEarnings { get; }

        public PublicUserData(string linoname, string displayname, PartnerStatus partnerStatus, string effect, bool deactivated, AboutPanel[] panels, Uri avatar, long followerCount, long chest, long balance, long earnings)
        {
            Linoname = linoname;
            Displayname = displayname;
            PartnerStatus = partnerStatus;
            Effect = effect;
            Deactivated = deactivated;
            Panels = panels;
            AvatarUri = avatar;
            NumFollowers = followerCount;
            ChestValue = (float)chest / 100000;
            LemonBalance = LinoBalance = (float)balance / 100000;
            LemonEarnings = LinoEarnings = (float)earnings / 100000;
        }
    }
}