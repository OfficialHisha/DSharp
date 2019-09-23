using System;

namespace DSharp
{
    public struct PublicUserData
    {
        public string Linoname { get; }
        public string Displayname { get; }
        public PartnerStatus PartnerStatus { get; }
        public Uri AvatarUri { get; }
        public long NumFollowers { get; }

        public PublicUserData(string linoname, string displayname, PartnerStatus partnerStatus, Uri avatar, long followerCount)
        {
            Linoname = linoname;
            Displayname = displayname;
            PartnerStatus = partnerStatus;
            AvatarUri = avatar;
            NumFollowers = followerCount;
        }
    }
}