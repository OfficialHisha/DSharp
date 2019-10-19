using GraphQL.Client;
using GraphQL.Common.Response;

namespace DSharp.Dlive.Query
{
    public static class PublicQuery
    {
        private static GraphQLClient _publicClient = new GraphQLClient(Dlive.QueryEndpoint);
        
        public static PublicUserData GetPublicInfoByDisplayname(string displayname)
        {
            GraphQLResponse response = _publicClient.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.USER_BY_DISPLAYNAME, displayname)).Result;
            
            RawUserData userData = response.GetDataFieldAs<RawUserData>("userByDisplayName");

            return userData.ToPublicUserData();
        }
        
        public static PublicUserData GetPublicInfo(string username)
        {
            GraphQLResponse response = _publicClient.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.USER, username)).Result;
            
            RawUserData userData = response.GetDataFieldAs<RawUserData>("user");

            return userData.ToPublicUserData();
        }
    }
}