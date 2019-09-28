using System;

namespace DSharp.Utility
{
    public static class Util
    {
        public static DateTime EpocMSToDateTime(double epoc)
        {
            DateTime epocTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return epocTime.AddMilliseconds(epoc).ToLocalTime();
        }

        public static PublicUserData UserObjectToPublicUserData(dynamic userObject)
        {
            Enum.TryParse(userObject.partnerStatus.ToString().ToUpper(), out PartnerStatus partnerStatus);
            return new PublicUserData(userObject.username.ToString(), userObject.displayname.ToString(), partnerStatus, false, null, new Uri(userObject.avatar.ToString()), -1, -1, -1, -1);
        }
    }
}
