using System;
using DSharp.GraphqlHelpers;
using GraphQL.Common.Response;
using Newtonsoft.Json.Linq;

namespace DSharp.Query
{
    public static class Query
    {
        private struct InternalUserData
        {
            public string username;
            public string displayname;
            public PartnerStatus partnerStatus;
            public Uri avatar;
            public JObject followers;
            public InternalPrivateData @private;
        }

        private struct InternalPrivateData
        {
            public JObject subscribers;
            public string email;
            public bool emailVerified;
            public string[] filterWords;
            public JObject streamKey;
        }

        public static UserData GetMyInfo()
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authorization token not provided. You can't use this query without authorization'");

            GraphQLResponse response = Dlive.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.ME)).Result;

            InternalUserData userData = response.GetDataFieldAs<InternalUserData>("me");

            return new UserData(userData.username, userData.displayname, userData.partnerStatus, userData.avatar, (long)userData.followers["totalCount"],
                                (long)userData.@private.subscribers["totalCount"], userData.@private.email, userData.@private.filterWords, userData.@private.streamKey["key"].ToString());
        }

        public static PublicUserData GetPublicInfoByDisplayname(string displayname)
        {
            GraphQLResponse response = Dlive.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.USER_BY_DISPLAYNAME, displayname)).Result;
            
            InternalUserData userData = response.GetDataFieldAs<InternalUserData>("userByDisplayName");
            
            return new PublicUserData(userData.username, userData.displayname, userData.partnerStatus, userData.avatar, (long)userData.followers["totalCount"]);
        }
        
        public static PublicUserData GetPublicInfo(string username)
        {
            GraphQLResponse response = Dlive.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.USER, username)).Result;
            
            InternalUserData userData = response.GetDataFieldAs<InternalUserData>("user");

            return new PublicUserData(userData.username, userData.displayname, userData.partnerStatus, userData.avatar, (long)userData.followers["totalCount"]);
        }
    }
}