using System.ComponentModel;
using System.Text;

namespace DSharp.Dlive.Query
{
    public static class GraphqlHelper
    {
        public static string GetQueryString(QueryType queryType, string[] data = null)
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
                    user.Append($"user(username:\"{data[0]}\") {{");
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
                    userByDisplayname.Append($"userByDisplayName(displayname:\"{data[0]}\") {{");
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
                case QueryType.FOLLOWERS:
                    StringBuilder followers = new StringBuilder();
                    followers.Append("query{");
                    followers.Append($"userByDisplayName(displayname:\"{data[0]}\") {{");
                    followers.Append($"followers (first: {data[1]}, after: \"{data[2]}\") {{");
                    followers.Append(@"totalCount
                        list {
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
                            }}}}");
                    return followers.ToString();
                case QueryType.REPLAYS:
                    return "";
                default:
                    throw new InvalidEnumArgumentException($"Invalid query type provided ({nameof(queryType)})");
            }
        }
    }
}