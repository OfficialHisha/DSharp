using System;
using System.Collections.Generic;
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
            public InternalPanelData[] panels;
            public InternalPrivateData @private;

            public UserData ToUserData()
            {
                List<AboutPanel> actualPanels = new List<AboutPanel>();
                foreach (InternalPanelData panel in panels)
                {
                    actualPanels.Add(panel.ToAboutPanel());
                }
                
                PublicUserData publicData = new PublicUserData(username, displayname, partnerStatus, actualPanels.ToArray(), avatar, (long)followers["totalCount"]);
                PrivateUserData privateData = new PrivateUserData((long) @private.subscribers["totalCount"],
                    @private.email, @private.filterWords, @private.streamKey["key"].ToString());
                
                return new UserData(publicData, privateData);
            }

            public PublicUserData ToPublicUserData()
            {
                List<AboutPanel> actualPanels = new List<AboutPanel>();
                foreach (InternalPanelData panel in panels)
                {
                    actualPanels.Add(panel.ToAboutPanel());
                }
                return new PublicUserData(username, displayname, partnerStatus, actualPanels.ToArray(), avatar, (long)followers["totalCount"]);
            }
        }

        private struct InternalPrivateData
        {
            public JObject subscribers;
            public string email;
            public bool emailVerified;
            public string[] filterWords;
            public JObject streamKey;
        }

        private struct InternalPanelData
        {
            public int id;
            public AboutPanelType type;
            public string title;
            public string body;
            public Uri imageURL;
            public Uri imageLinkURL;

            public AboutPanel ToAboutPanel()
            {
                return new AboutPanel(id, type, title, body, imageURL, imageLinkURL);
            }
        }

        public static UserData GetMyInfo()
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use this query. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLResponse response = Dlive.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.ME)).Result;

            InternalUserData userData = response.GetDataFieldAs<InternalUserData>("me");

            return userData.ToUserData();
        }

        public static PublicUserData GetPublicInfoByDisplayname(string displayname)
        {
            GraphQLResponse response = Dlive.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.USER_BY_DISPLAYNAME, displayname)).Result;
            
            InternalUserData userData = response.GetDataFieldAs<InternalUserData>("userByDisplayName");

            return userData.ToPublicUserData();
        }
        
        public static PublicUserData GetPublicInfo(string username)
        {
            GraphQLResponse response = Dlive.Client.SendQueryAsync(GraphqlHelper.GetQueryString(QueryType.USER, username)).Result;
            
            InternalUserData userData = response.GetDataFieldAs<InternalUserData>("user");

            return userData.ToPublicUserData();
        }
    }
}