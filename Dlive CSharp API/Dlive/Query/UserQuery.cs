using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.Common.Response;

namespace DSharp.Dlive.Query
{
    public class UserQuery
    {
        private DliveAccount _account;

        public UserQuery(DliveAccount account)
        {
            _account = account;
        }

        public UserData GetMyInfo()
        {
            if (!_account.IsAuthenticated)
            {
                _account.RaiseError("Authentication is required to use this query. Set the Dlive.AuthorizationToken property with your user token to authenticate");
                return new UserData();
            }

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse response = _account.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.ME)).Result;

            RawUserData userData = response.GetDataFieldAs<RawUserData>("me");

            return userData.ToUserData();
        }

        public PublicUserData GetPublicInfoByDisplayName(string displayName)
        {
            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse response = _account.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.USER_BY_DISPLAYNAME, new [] {displayName})).Result;
            
            RawUserData userData = response.GetDataFieldAs<RawUserData>("userByDisplayName");

            return userData.ToPublicUserData();
        }
        
        public PublicUserData GetPublicInfo(string username)
        {
            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse response = _account.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.USER, new [] {username})).Result;
            
            RawUserData userData = response.GetDataFieldAs<RawUserData>("user");

            return userData.ToPublicUserData();
        }

        /// <summary>
        /// This method might take some time to complete depending on the number of followers and other queries
        /// that are sent while it's running.
        /// </summary>
        /// <param name="user">The user object of the user you want to fetch followers from</param>
        /// <returns>An array of public user data objects, one for each follower</returns>
        public async Task<PublicUserData[]> GetFollowersForUser(PublicUserData user)
        {
            //followers (first = amount of followers to fetch, after = start after this NUMBER)
            int cursor = 0;

            List<PublicUserData> followers = new List<PublicUserData>();

            while (cursor < user.NumFollowers)
            {
                if (!Dlive.CanExecuteQuery())
                    await Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds);
                Dlive.IncreaseQueryCounter();

                GraphQLResponse response = _account.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.FOLLOWERS,
                    new string[] { user.Linoname, "50", cursor.ToString() })).Result;

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