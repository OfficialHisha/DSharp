using System;
using System.ComponentModel;
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
                                panels {
                                    id
                                    type
                                    title
                                    imageURL
                                    imageLinkURL
                                    body
                                }
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
                        panels {
                            id
                            type
                            title
                            imageURL
                            imageLinkURL
                            body
                        }
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
                        panels {
                            id
                            type
                            title
                            imageURL
                            imageLinkURL
                            body
                        }
                        followers {
                            totalCount
                        }}}");
                    return userByDisplayname.ToString();
                default:
                    throw new InvalidEnumArgumentException($"Invalid query type provided ({nameof(queryType)})");
            }
        }
    }
}