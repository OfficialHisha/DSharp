using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.Common.Request;
using GraphQL.Common.Response;

namespace DSharp.Mutation
{
    [Obsolete("Be aware that mutations are largely untested at this stage, use with caution")]
    public static class Mutation
    {
        public static void SendChatMessage(string username, string message)
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use mutations. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{sendStreamchatMessage(input:{{ streamer: \"{username}\", message: \"{message}\", roomRole: Owner, subscribing: true}}) {{ err {{ message }}}}}}"
            };
            
            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Data.err != null)
            {
                throw new Exception($"An error occured while sending chat message: {res.Data.err.message.ToString()}");
            }
        }

        public static void DeleteChatMessage(string username, string messageId)
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use mutations. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{chatDelete(streamer: \"{username}\", id: \"{messageId}\") {{ err {{ message }}}}}}"
            };
            
            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Data.err != null)
            {
                throw new Exception($"An error occured while sending chat message: {res.Data.err.message.ToString()}");
            }
        }

        public static void AddModerator(string newModeratorUsername)
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use mutations. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{moderatorUsername(username: \"{newModeratorUsername}\") {{ err {{ message }}}}}}"
            };
            
            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Data.err != null)
            {
                throw new Exception($"An error occured while sending chat message: {res.Data.err.message.ToString()}");
            }
        }

        public static void RemoveModerator(string moderatorUsername)
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use mutations. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{moderatorRemove(username: \"{moderatorUsername}\") {{ err {{ message }}}}}}"
            };
            
            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Data.err != null)
            {
                throw new Exception($"An error occured while sending chat message: {res.Data.err.message.ToString()}");
            }
        }

        public static void AddAboutPanel(string title = "New Panel", string content = "No content added", Uri image = null, Uri imageDestination = null, AboutPanelType type = AboutPanelType.DEFAULT)
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use mutations. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{panelAdd(input:{{ type: {type} }}) {{ err {{ message }} panel {{ id }}}}}}"
            };
            
            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Data.err != null)
            {
                throw new Exception($"An error occured while creating a new panel: {res.Data.err.message.ToString()}");
            }

            UpdateAboutPanel(new AboutPanel(int.Parse(res.Data.panel.id.ToString()), type, title, content, image, imageDestination));
        }

        public static void AddAboutPanel(AboutPanel panel)
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use mutations. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{panelAdd(input:{{ type: {panel.PanelType} }}) {{ err {{ message }} panel {{ id }}}}}}"
            };
            
            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Data.err != null)
            {
                throw new Exception($"An error occured while creating a new panel: {res.Data.err.message.ToString()}");
            }

            UpdateAboutPanel(new AboutPanel(int.Parse(res.Data.panel.id.ToString()), panel.PanelType, panel.PanelTitle, panel.PanelText, panel.PanelImageUrl, panel.PanelUrlDestination));
        }

        public static void UpdateAboutPanel(AboutPanel panel)
        {
            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{panelUpdate(input:{{ id: {panel.PanelId}, title: {panel.PanelTitle}, imageURL: {panel.PanelImageUrl}, imageLinkURL: {panel.PanelUrlDestination}, body: {panel.PanelText} }}) {{ err {{ message }}}}}}"
            };
            
            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Data.err != null)
            {
                throw new Exception($"An error occured while applying new panel values: {res.Data.err.message.ToString()}");
            }
        }

        public static void DeleteAboutPanel(int id)
        {
            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{panelDelete(input:{{ id: {id} }}) {{ err {{ message }}}}}}"
            };
            
            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Data.err != null)
            {
                throw new Exception($"An error occured while deleting a panel: {res.Data.err.message.ToString()}");
            }
        }
        
        public static void DeleteAboutPanel(AboutPanel panel)
        {
            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{panelDelete(input:{{ id: {panel.PanelId} }}) {{ err {{ message }}}}}}"
            };
            
            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Data.err != null)
            {
                throw new Exception($"An error occured while deleting a panel: {res.Data.err.message.ToString()}");
            }
        }

        public static void SetAboutPanelOrder(int[] panelIds)
        {
            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{panelDelete(input:{{ ids: {panelIds} }}) {{ err {{ message }}}}}}"
            };
            
            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Data.err != null)
            {
                throw new Exception($"An error occured while ordering panels: {res.Data.err.message.ToString()}");
            }
        }
        
        public static void SetAboutPanelOrder(AboutPanel[] panels)
        {
            List<int> ids = new List<int>();

            foreach (AboutPanel panel in panels)
            {
                ids.Add(panel.PanelId);
            }
            
            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{panelDelete(input:{{ ids: {ids.ToArray()} }}) {{ err {{ message }}}}}}"
            };
            
            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Data.err != null)
            {
                throw new Exception($"An error occured while ordering panels: {res.Data.err.message.ToString()}");
            }
        }
    }
}
