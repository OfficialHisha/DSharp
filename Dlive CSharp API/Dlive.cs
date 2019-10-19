using System;

namespace DSharp.Dlive
{
    public static class Dlive
    {
        public static Uri QueryEndpoint { get; } = new Uri("https://graphigo.prd.dlive.tv/");
        public static Uri SubscriptionEndpoint { get; } = new Uri("wss://graphigostream.prd.dlive.tv/");
    }
}