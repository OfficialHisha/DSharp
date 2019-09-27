using System;
using Newtonsoft.Json.Linq;

namespace DSharp
{
    public struct PublicUserData
    {
        public string Linoname { get; }
        public string Displayname { get; }
        public PartnerStatus PartnerStatus { get; }
        public AboutPanel[] Panels { get; }
        public Uri AvatarUri { get; }
        public long NumFollowers { get; }

        public PublicUserData(string linoname, string displayname, PartnerStatus partnerStatus, AboutPanel[] panels, Uri avatar, long followerCount)
        {
            Linoname = linoname;
            Displayname = displayname;
            PartnerStatus = partnerStatus;
            Panels = panels;
            AvatarUri = avatar;
            NumFollowers = followerCount;
        }
    }
}