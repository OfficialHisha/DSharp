using System;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using GraphQL.Client.Http;

namespace DSharp
{
    public static class Dlive
    {
        public static Uri QueryEndpoint { get; } = new Uri("https://graphigo.prd.dlive.tv/");
        public static Uri SubscriptionEndpoint { get; } = new Uri("wss://graphigostream.prd.dlive.tv/");
        
        public static bool IsAuthenticated { get; private set; }
        
        public static GraphQLHttpClient Client { get; } = new GraphQLHttpClient(QueryEndpoint);
        
        public static string AuthorizationToken
        {
            set
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(value);
                IsAuthenticated = true;
            }
        }
    }
}