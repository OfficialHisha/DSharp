using System.ComponentModel;
using System.Text;

namespace DSharp.Dlive.Query
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
                                deactivated
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
                                }
                                wallet {
                                    totalEarning
                                    balance
                                }
                                treasureChest {
                                    value
                                }
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
                        deactivated
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
                        }
                        wallet {
                            totalEarning
                            balance
                        }
                        treasureChest {
                            value
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
                        deactivated
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
                        }
                        wallet {
                            totalEarning
                            balance
                        }
                        treasureChest {
                            value
                        }}}");
                    return userByDisplayname.ToString();
                default:
                    throw new InvalidEnumArgumentException($"Invalid query type provided ({nameof(queryType)})");
            }
        }
    }
}