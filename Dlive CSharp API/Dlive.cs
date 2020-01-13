using System;

namespace DSharp.Dlive
{
    public static class Dlive
    {
        public static bool EnableRateLimiter { get; set; }
        
        private static DateTime currentIntervalStart;
        private static int currentIntervalQueries;
        
        public static Uri QueryEndpoint { get; } = new Uri("https://graphigo.prd.dlive.tv/");
        public static Uri SubscriptionEndpoint { get; } = new Uri("wss://graphigostream.prd.dlive.tv/");

        public static bool CanExecuteQuery()
        {
            TimeSpan ts = new TimeSpan(0, 0, 5, 0);
            
            if (DateTime.Now > currentIntervalStart + ts)
            {
                currentIntervalStart = DateTime.Now;
                currentIntervalQueries = 0;
            }

            return currentIntervalQueries < 5000;
        }
    }
}