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
    }
}
