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
            {
                _account.RaiseError("Rate limit reached");
                return new UserData();
            }
            
            GraphQLResponse response = _account.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.ME)).Result;

            RawUserData userData = response.GetDataFieldAs<RawUserData>("me");

            return userData.ToUserData();
        }

        public PublicUserData GetPublicInfoByDisplayName(string displayName)
        {
            if (!Dlive.CanExecuteQuery())
            {
                _account.RaiseError("Rate limit reached");
                return new PublicUserData();
            }
            
            GraphQLResponse response = _account.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.USER_BY_DISPLAYNAME, new [] {displayName})).Result;
            
            RawUserData userData = response.GetDataFieldAs<RawUserData>("userByDisplayName");

            return userData.ToPublicUserData();
        }
        
        public PublicUserData GetPublicInfo(string username)
        {
            if (!Dlive.CanExecuteQuery())
            {
                _account.RaiseError("Rate limit reached");
                return new PublicUserData();
            }
            
            GraphQLResponse response = _account.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.USER, new [] {username})).Result;
            
            RawUserData userData = response.GetDataFieldAs<RawUserData>("user");

            return userData.ToPublicUserData();
        }
    }
}