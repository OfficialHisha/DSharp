using GraphQL.Client.Http;
using GraphQL.Common.Response;
using System;

namespace DSharp.Dlive.Query
{
    public static class PublicQuery
    {
        private static GraphQLHttpClient _publicClient = new GraphQLHttpClient(Dlive.QueryEndpoint);

        [Obsolete("Use GetPublicInfoByDisplayName instead")]
        public static PublicUserData GetPublicInfoByDisplayname(string displayname)
        {
            return GetPublicInfoByDisplayName(displayname);
        }

        public static PublicUserData GetPublicInfoByDisplayName(string displayname)
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