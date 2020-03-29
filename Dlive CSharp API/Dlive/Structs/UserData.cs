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

        public UserData(string username, string displayname, PartnerStatus partnerStatus, string effect, Badge[] badges, bool deactivated, AboutPanel[] panels, Uri avatar, long followerCount,
                        long chest, long balance, long totalEarnings, long subscriberCount, string email, string[] wordFilter, string streamKey)
        {
            Public = new PublicUserData(username, displayname, partnerStatus, effect, badges, deactivated, panels, avatar, followerCount, chest, balance, totalEarnings);
            Private = new PrivateUserData(subscriberCount, email, wordFilter, streamKey);
        }
    }
}