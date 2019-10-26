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
                _account.RaiseError("Authentication is required to use this query. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLResponse response = _account.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.ME)).Result;

            RawUserData userData = response.GetDataFieldAs<RawUserData>("me");

            return userData.ToUserData();
        }

        public PublicUserData GetPublicInfoByDisplayName(string displayName)
        {
            GraphQLResponse response = _account.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.USER_BY_DISPLAYNAME, displayName)).Result;
            
            RawUserData userData = response.GetDataFieldAs<RawUserData>("userByDisplayName");

            return userData.ToPublicUserData();
        }
        
        public PublicUserData GetPublicInfo(string username)
        {
            GraphQLResponse response = _account.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.USER, username)).Result;
            
            RawUserData userData = response.GetDataFieldAs<RawUserData>("user");

            return userData.ToPublicUserData();
        }
    }
}