using System;
using System.Collections.Generic;
using DSharp.Dlive;
using DSharp.Dlive.Query;

namespace DSharp.Utility
{
    public static class Util
    {
        public static DateTime EpocMSToDateTime(double epoc)
        {
            DateTime epocTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return epocTime.AddMilliseconds(epoc).ToLocalTime();
        }

        public static PublicUserData DliveUserObjectToPublicUserData(dynamic userObject)
        {
            Enum.TryParse(userObject.partnerStatus.ToString().ToUpper(), out PartnerStatus partnerStatus);

            List<Badge> badges = new List<Badge>();
            foreach (dynamic badgeObject in userObject.badges)
            {
                Enum.TryParse(badgeObject.ToString().ToUpper(), out Badge badge);
                badges.Add(badge);
            }

            return new PublicUserData(userObject.username.ToString(), userObject.displayname.ToString(), partnerStatus, userObject.effect.ToString(), badges.ToArray(), false, null, new Uri(userObject.avatar.ToString()), -1, -1, -1, -1);
        }

        public static string DliveUsernameToDisplayName(string username)
        {
            return PublicQuery.GetPublicInfo(username).Displayname;
        }
        
        public static string DliveDisplayNameToUsername(string displayName)
        {
            return PublicQuery.GetPublicInfoByDisplayName(displayName).Linoname;
        }
    }
}
