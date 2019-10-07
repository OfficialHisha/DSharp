using System;

namespace DSharp
{
    public struct PublicUserData
    {
        public string Linoname { get; }
        public string Displayname { get; }
        public PartnerStatus PartnerStatus { get; }
        public bool Deactivated { get; }
        public AboutPanel[] Panels { get; }
        public Uri AvatarUri { get; }
        public long NumFollowers { get; }
        public float ChestValue { get; }
        public long LinoBalance { get; }
        public long LinoEarnings { get; }

        public PublicUserData(string linoname, string displayname, PartnerStatus partnerStatus, bool deactivated, AboutPanel[] panels, Uri avatar, long followerCount, long chest, long balance, long earnings)
        {
            Linoname = linoname;
            Displayname = displayname;
            PartnerStatus = partnerStatus;
            Deactivated = deactivated;
            Panels = panels;
            AvatarUri = avatar;
            NumFollowers = followerCount;
            ChestValue = (float)chest / 100000;
            LinoBalance = balance;
            LinoEarnings = earnings;
        }
    }
}