using System;

namespace DSharp.Dlive
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
        
        public UserData(string linoname, string displayname, PartnerStatus partnerStatus, string effect, bool deactivated, AboutPanel[] panels, Uri avatar, long followerCount,
                        long chest, long balance, long totalEarnings, long subscriberCount, string email, string[] wordFilter, string streamKey)
        {
            Public = new PublicUserData(linoname, displayname, partnerStatus, effect, deactivated, panels, avatar, followerCount, chest, balance, totalEarnings);
            Private = new PrivateUserData(subscriberCount, email, wordFilter, streamKey);
        }
    }
}