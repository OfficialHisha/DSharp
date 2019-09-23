using System;
using System.Text;
using DSharp.Query;

namespace DSharp.GraphqlHelpers
{
    public static class GraphqlHelper
    {
        public static string GetQueryString(QueryType queryType, string data = "")
        {
            switch (queryType)
            {
                case QueryType.ME:
                    return @"query{
                            me {
                                username
                                displayname
                                avatar
                                partnerStatus
                                followers {
                                    totalCount
                                private {
                                    email
                                    subscribers {
                                        totalCount
                                    }
                                    filterWords
                                    streamKey {
                                        key
                                    }}}}}";
                case QueryType.USER:
                    StringBuilder user = new StringBuilder();
                    user.Append("query{");
                    user.Append($"user(username:\"{data}\") {{");
                    user.Append(@"username
                        displayname
                        avatar
                        partnerStatus
                        followers {
                            totalCount
                        }}}");
                    return user.ToString();
                case QueryType.USER_BY_DISPLAYNAME:
                    StringBuilder userByDisplayname = new StringBuilder();
                    userByDisplayname.Append("query{");
                    userByDisplayname.Append($"userByDisplayName(displayname:\"{data}\") {{");
                    userByDisplayname.Append(@"username
                        displayname
                        avatar
                        partnerStatus
                        followers {
                            totalCount
                        }}}");
                    return userByDisplayname.ToString();
                default:
                    throw new ArgumentOutOfRangeException(nameof(queryType), queryType, "Invalid query type provided");
            }
        }
    }
}