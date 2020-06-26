using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharp.Dlive.Exceptions;
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

            if (response.Data.me == null)
                throw new AccountException($"User data was not received, this could be caused by an expired user token!");

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
            
            do
            {
                if (!Dlive.CanExecuteQuery())
                    await Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds);
                Dlive.IncreaseQueryCounter();

                GraphQLResponse response = _account.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.SUBSCRIBERS,
                    new string[] { userData.Public.Username, "50", cursor.ToString() })).Result;

                JArray subscriberList = response.Data.me.@private.subscribers.list;

                foreach (JObject subscriber in subscriberList)
                {
                    RawUserData subscriberData = new RawUserData(subscriber["subscriber"] as JObject);
                    subscribers.Add(subscriberData.ToPublicUserData());
                }

                cursor += 50;
                if (userData.Private.SubscriberCount > 50)
                    await Task.Delay(200);
            }
            while (cursor < userData.Private.SubscriberCount);

            return subscribers.ToArray();
        }

        public PublicUserData GetPublicInfoByDisplayName(string displayName)
        {
            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse response = _account.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.USER_BY_DISPLAYNAME, new [] {displayName})).Result;

            PublicUserData userData;
            if (response.Data.userByDisplayName != null)
            {
                userData = response.GetDataFieldAs<RawUserData>("userByDisplayName").ToPublicUserData();
            }
            else
            {
                userData = new PublicUserData("invalid user", "Invalid User", PartnerStatus.NONE, false, "", null, true, null, null, 0, 0, 0, 0);
            }

            return userData;
        }
        
        public PublicUserData GetPublicInfo(string username)
        {
            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse response = _account.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.USER, new [] {username})).Result;

            PublicUserData userData;
            if (response.Data.user != null)
            {
                userData = response.GetDataFieldAs<RawUserData>("user").ToPublicUserData();
            }
            else
            {
                userData = new PublicUserData("invalid user", "Invalid User", PartnerStatus.NONE, false, "", null, true, null, null, 0, 0, 0, 0);
            }

            return userData;
        }

        /// <summary>
        /// This method might take some time to complete depending on the number of followers and other queries
        /// that are sent while it's running.
        /// </summary>
        /// <returns>An array of public user data objects, one for each follower</returns>
        [Obsolete("There is a Dlive bug that causes this method to, possibly, miss some followers. There is nothing I can do about it until Dlive fixes their API. Sorry :(")]
        public async Task<PublicUserData[]> GetFollowers()
        {
            int cursor = -1;

            List<PublicUserData> followers = new List<PublicUserData>();

            PublicUserData userData = _account.Query.GetMyInfo().Public;

            if (userData.NumFollowers == 0)
            {
                return followers.ToArray();
            }

            do
            {
                if (!Dlive.CanExecuteQuery())
                    await Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds);
                Dlive.IncreaseQueryCounter();

                GraphQLResponse response = _account.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.FOLLOWERS,
                    new string[] { userData.Username, "50", cursor.ToString() })).Result;

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