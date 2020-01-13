using GraphQL.Client.Http;
using GraphQL.Common.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DSharp.Dlive.Query
{
    public static class PublicQuery
    {
        private static GraphQLHttpClient _publicClient = new GraphQLHttpClient(Dlive.QueryEndpoint);

        public static PublicUserData GetPublicInfoByDisplayName(string displayname)
        {
            GraphQLResponse response = _publicClient.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.USER_BY_DISPLAYNAME, new string[] {displayname})).Result;
            
            RawUserData userData = response.GetDataFieldAs<RawUserData>("userByDisplayName");

            return userData.ToPublicUserData();
        }
        
        public static PublicUserData GetPublicInfo(string username)
        {
            GraphQLResponse response = _publicClient.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.FOLLOWERS, new string[] {username})).Result;
            
            RawUserData userData = response.GetDataFieldAs<RawUserData>("user");

            return userData.ToPublicUserData();
        }

        /// <summary>
        /// This method might take some time to complete depending on the number of followers and other queries
        /// that are sent while it's running.
        /// </summary>
        /// <param name="user">The user object of the user you want to fetch followers from</param>
        /// <returns>An array of public user data objects, one for each follower</returns>
        public static async Task<PublicUserData[]> GetFollowersForUser(PublicUserData user)
        {
            //followers (first = amount of followers to fetch, after = start after this NUMBER)
            int cursor = 0;
            
            List<PublicUserData> followers = new List<PublicUserData>();

            while (cursor < user.NumFollowers)
            {
                GraphQLResponse response = _publicClient.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.FOLLOWERS,
                    new string[] {user.Linoname, "50", cursor.ToString()})).Result;

                RawUserData[] userData = response.GetDataFieldAs<RawUserData[]>("user.followers.list");

                
                foreach (RawUserData rawuserData in userData)
                {
                    followers.Add(rawuserData.ToPublicUserData());
                }

                cursor += 50;
                await Task.Delay(1000);
            }

            

            return followers.ToArray();
        }
    }
}