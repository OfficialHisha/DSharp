using System;
using System.Net.Http.Headers;
using DSharp.Dlive.Query;
using GraphQL.Client.Http;

namespace DSharp.Dlive
{
    public class DliveAccount
    {
        public GraphQLHttpClient Client { get; } = new GraphQLHttpClient(Dlive.QueryEndpoint);
        
        public UserQuery Query { get; }
        public Mutation.Mutation Mutation { get; }

        public bool IsAuthenticated { get; private set; }

        public event Action<string> OnError;

        public string AuthorizationToken
        {
            set
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(value);
                IsAuthenticated = true;
            }
        }
        
        public DliveAccount(string authorizationToken) : this()
        {
            AuthorizationToken = authorizationToken;
        }

        public DliveAccount()
        {
            Query = new UserQuery(this);
            Mutation = new Mutation.Mutation(this);
        }

        public void RaiseError(string message)
        {
            OnError?.Invoke(message);
            Dlive.AddLogEntry(LogLevel.ERROR, message);
        }
    }
}