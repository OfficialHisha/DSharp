using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.Common.Response;
using Newtonsoft.Json.Linq;

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

        /// <summary>
        /// This method might take some time to complete depending on the number of subscribers and other queries
        /// that are sent while it's running.
        /// </summary>
        /// <returns>An array of public user data objects, one for each subscriber</returns>
        public async Task<PublicUserData[]> GetSubscribers()
        {
            List<PublicUserData> subscribers = new List<PublicUserData>();

            if (!_account.IsAuthenticated)
            {
                _account.RaiseError("Authentication is required to use this query. Set the Dlive.AuthorizationToken property with your user token to authenticate");
                return subscribers.ToArray();
            }

            int cursor = -1;

            UserData userData = _account.Query.GetMyInfo();

            if (userData.Private.SubscriberCount == 0)
            {
                return subscribers.ToArray();
            }

            while (cursor < userData.Private.SubscriberCount)
            {
                if (!Dlive.CanExecuteQuery())
                    await Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds);
                Dlive.IncreaseQueryCounter();

                GraphQLResponse response = _account.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.SUBSCRIBERS,
                    new string[] { userData.Public.Linoname, "50", cursor.ToString() })).Result;

                Console.WriteLine(response.Data.ToString());

                JArray subscriberList = response.Data.me.@private.subscribers.list;

                Console.WriteLine(subscriberList.Count);

                foreach (JObject subscriber in subscriberList["subscriber"])
                {
                    RawUserData subscriberData = new RawUserData(subscriber);
                    subscribers.Add(subscriberData.ToPublicUserData());
                }

                cursor += 50;
                await Task.Delay(1000);
            }

            return subscribers.ToArray();
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
        /// <returns>An array of public user data objects, one for each follower</returns>
        public async Task<PublicUserData[]> GetFollowers()
        {
            int cursor = -1;

            List<PublicUserData> followers = new List<PublicUserData>();

            PublicUserData userData = _account.Query.GetMyInfo().Public;

            if (userData.NumFollowers == 0)
            {
                return followers.ToArray();
            }

            while (cursor < userData.NumFollowers)
            {
                if (!Dlive.CanExecuteQuery())
                    await Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds);
                Dlive.IncreaseQueryCounter();

                GraphQLResponse response = _account.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.FOLLOWERS,
                    new string[] { userData.Linoname, "50", cursor.ToString() })).Result;

                JArray followerList = response.Data.user.followers.list;

                foreach (JObject follower in followerList)
                {
                    RawUserData followerData = new RawUserData(follower);
                    followers.Add(followerData.ToPublicUserData());
                }

                cursor += 50;
                await Task.Delay(1000);
            }

            return followers.ToArray();
        }
    }
}