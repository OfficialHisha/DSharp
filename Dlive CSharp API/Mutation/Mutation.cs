using System;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using GraphQL.Client.Http;
using GraphQL.Common.Request;
using GraphQL.Common.Response;

namespace DSharp
{
    public static class Mutation
    {
        public static bool IsAuthenticated { get; private set; }
        public static string Authentication {
            set {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(value);
                IsAuthenticated = true;
            }
        }
        
        static Uri _uri = new Uri("https://graphigo.prd.dlive.tv/");

        static GraphQLHttpClient _client = new GraphQLHttpClient(_uri);

        public static void SendChatMessage(string username, string message)
        {
            if (!IsAuthenticated)
                throw new Exception("Authentication is required to send chat messages. Set the Authentication property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{sendStreamchatMessage(input:{{ streamer: \"{username}\", message: \"{message}\", roomRole: Owner, subscribing: true}}) {{ err {{ message }}}}}}"
            };
            
            GraphQLResponse res = Task.Run(() => _client.SendMutationAsync(_req)).Result;

            if (res.Data.err != null)
            {
                throw new Exception($"An error occured while sending chat message: {res.Data.err.message.ToString()}");
            }
        }
    }
}
