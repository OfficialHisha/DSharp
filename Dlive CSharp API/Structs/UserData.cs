using System;
using Newtonsoft.Json.Linq;

namespace DSharp
{
    public struct UserData
    {
        public PublicUserData Public { get; }
        public PrivateUserData Private { get; }

        public UserData(PublicUserData publicUserData, PrivateUserData privateUserData)
        {
            Public = publicUserData;
            Private = privateUserData;
        }
        
        public UserData(string linoname, string displayname, PartnerStatus partnerStatus, AboutPanel[] panels, Uri avatar,
                        long followerCount, long subscriberCount, string email, string[] wordFilter, string streamKey)
        {
            Public = new PublicUserData(linoname, displayname, partnerStatus, panels, avatar, followerCount);
            Private = new PrivateUserData(subscriberCount, email, wordFilter, streamKey);
        }
    }
}