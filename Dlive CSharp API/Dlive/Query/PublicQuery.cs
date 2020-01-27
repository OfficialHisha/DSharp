using GraphQL.Client.Http;
using GraphQL.Common.Response;
using Newtonsoft.Json.Linq;
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
            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse response = _publicClient.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.USER_BY_DISPLAYNAME, new string[] {displayname})).Result;
            
            RawUserData userData = response.GetDataFieldAs<RawUserData>("userByDisplayName");

            return userData.ToPublicUserData();
        }
        
        public static PublicUserData GetPublicInfo(string username)
        {
            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse response = _publicClient.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.USER, new string[] { username })).Result;
           
            RawUserData userData = response.GetDataFieldAs<RawUserData>("user");

            return userData.ToPublicUserData();
        }

        /// <summary>
        /// This method might take some time to complete depending on the number of followers and other queries
        /// that are sent while it's running.
        /// </summary>
        /// <param name="user">The user object of the user you want to fetch followers from</param>
        /// <returns>An array of public user data objects, one for each follower</returns>
        [Obsolete("There is a Dlive bug that causes this method to, possibly, miss some followers. There is nothing I can do about it until Dlive fixes their API. Sorry :(")]
        public static async Task<PublicUserData[]> GetFollowersForUser(string username)
        {
            int cursor = -1;
            
            List<PublicUserData> followers = new List<PublicUserData>();

            PublicUserData userData = GetPublicInfo(username);

            if (userData.NumFollowers == 0)
            {
                return followers.ToArray();
            }

            do
            {
                if (!Dlive.CanExecuteQuery())
                    await Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds);
                Dlive.IncreaseQueryCounter();

                GraphQLResponse response = _publicClient.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.FOLLOWERS,
                    new string[] { userData.Linoname, "50", cursor.ToString() })).Result;

                JArray followerList = response.Data.user.followers.list;

                foreach (JObject follower in followerList)
                {
                    RawUserData followerData = new RawUserData(follower);
                    followers.Add(followerData.ToPublicUserData());
                }

                cursor += 50;
                if (userData.NumFollowers > 50)
                    await Task.Delay(200);
            }
            while (cursor < userData.NumFollowers);

            return followers.ToArray();
        }    
    }
}