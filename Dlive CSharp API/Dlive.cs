using System;

namespace DSharp.Dlive
{
    public static class Dlive
    {
        public static string Version { get; } = "0.6.0";

        public static bool EnableRateLimiter { get; set; } = false;

        public static double LemonUSDValue { get; } = 0.012;

        public static DateTime NextIntervalReset { get; private set; } = DateTime.Now.AddMinutes(5);
        private static int currentIntervalQueries;
        
        public static Uri QueryEndpoint { get; } = new Uri("https://graphigo.prd.dlive.tv/");
        public static Uri SubscriptionEndpoint { get; } = new Uri("wss://graphigostream.prd.dlive.tv/");

        public static bool CanExecuteQuery()
        {
            if (!EnableRateLimiter)
                return true;
            
            if (DateTime.Now > NextIntervalReset)
            {
                NextIntervalReset = DateTime.Now.AddMinutes(5);
                currentIntervalQueries = 0;
            }

            return currentIntervalQueries < 5000;
        }

        public static void IncreaseQueryCounter()
        {
            currentIntervalQueries++;
        }
    }
}