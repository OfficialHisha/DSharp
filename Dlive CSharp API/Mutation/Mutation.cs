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

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while sending chat message: {res.Errors[0].Message}");
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

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while deleting chat message: {res.Errors[0].Message}");
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

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while adding moderator: {res.Errors[0].Message}");
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

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while removing moderator: {res.Errors[0].Message}");
            }
        }

        public static void BanUser(string streamer, string banUser)
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use mutations. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{streamchatUserBan(streamer: \"{streamer}\", username: \"{banUser}\") {{ err {{ message }}}}}}"
            };

            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while banning user: {res.Errors[0].Message}");
            }
        }

        public static void UnbanUser(string streamer, string banUser)
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use mutations. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{streamchatUserUnban(streamer: \"{streamer}\", username: \"{banUser}\") {{ err {{ message }}}}}}"
            };

            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while unbanning user: {res.Errors[0].Message}");
            }
        }

        //TODO: Figure out granularity of duration (assuming seconds for now, but it could as well be minutes)
        public static void TimeoutUser(string streamer, string timeoutUser, int seconds)
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use mutations. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{userTimeoutSet(streamer: \"{streamer}\", username: \"{timeoutUser}\", duration: {seconds}) {{ err {{ message }}}}}}"
            };

            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while setting timeout for user: {res.Errors[0].Message}");
            }
        }

        public static void SetChatMode(ChatMode newChatMode)
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use mutations. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{chatModeSet(chatMode: {newChatMode}) {{ err {{ message }}}}}}"
            };

            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while changing chatmode: {res.Errors[0].Message}");
            }
        }

        public static void SetEmoteMode(bool disallowPersonalEmotes, bool disallowChannelEmotes, bool disallowGlobalEmotes)
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use mutations. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{emoteModeSet(emoteMode:{{ NoMineEmote: {disallowGlobalEmotes}, NoAllEmote: {disallowChannelEmotes}, NoGlobalEmote: {disallowGlobalEmotes}}}) {{ err {{ message }}}}}}"
            };

            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while changing emoteMode: {res.Errors[0].Message}");
            }
        }

        public static void SetChatCooldown(int cooldownSeconds)
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use mutations. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{chatIntervalSet(seconds: {cooldownSeconds}) {{ err {{ message }}}}}}"
            };

            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while changing chat cooldown: {res.Errors[0].Message}");
            }
        }

        public static void AddFilterWord(string word)
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use mutations. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{filterWordAdd(word: \"{word}\") {{ err {{ message }}}}}}"
            };

            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while adding filtered word: {res.Errors[0].Message}");
            }
        }

        public static void RemoveFilterWord(string word)
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use mutations. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{filterWordDelete(word: \"{word}\") {{ err {{ message }}}}}}"
            };

            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while removing filtered word: {res.Errors[0].Message}");
            }
        }

        public static void BanEmote(string streamer, string emoteString)
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use mutations. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{emoteBan(streamer: \"{streamer}\", emoteStr: \"{emoteString}\") {{ err {{ message }}}}}}"
            };

            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while banning emote: {res.Errors[0].Message}");
            }
        }

        public static void UnbanEmote(string streamer, string emoteString)
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use mutations. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{emoteUnban(streamer: \"{streamer}\", emoteStr: \"{emoteString}\") {{ err {{ message }}}}}}"
            };

            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while unbanning emote: {res.Errors[0].Message}");
            }
        }

        public static void SetSubSettings(string badgeText, string badgeColor, string textColor, string[] benefits)
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use mutations. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{subSettingSet(subSetting:{{ badgeText: \"{badgeText}\", badgeColor: \"{badgeColor}\", textColor: \"{textColor}\", benefits: {benefits} }}) {{ err {{ message }}}}}}"
            };

            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while setting sub settings: {res.Errors[0].Message}");
            }
        }

        public static void OpenChest()
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use mutations. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{giveawayStart() {{ err {{ message }}}}}}"
            };

            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while opening chest: {res.Errors[0].Message}");
            }
        }

        public static void ClaimChest(string streamer)
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use mutations. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{giveawayClaim(streamer: \"{streamer}\") {{ err {{ message }}}}}}"
            };

            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while claiming chest: {res.Errors[0].Message}");
            }
        }

        public static void Follow(string streamer)
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use mutations. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{follow(streamer: \"{streamer}\") {{ err {{ message }}}}}}"
            };

            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while following user: {res.Errors[0].Message}");
            }
        }

        public static void Unfollow(string streamer)
        {
            if (!Dlive.IsAuthenticated)
                throw new AuthorizationException("Authentication is required to use mutations. Set the Dlive.AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{unfollow(streamer: \"{streamer}\") {{ err {{ message }}}}}}"
            };

            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while unfollowing user: {res.Errors[0].Message}");
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

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while creating a new panel: {res.Errors[0].Message}");
            }

            UpdateAboutPanel(new AboutPanel(int.Parse(res.Data.panelAdd.panel.id.ToString()), type, title, content, image, imageDestination));
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

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while creating a new panel: {res.Errors[0].Message}");
            }

            UpdateAboutPanel(new AboutPanel(int.Parse(res.Data.panel.id.ToString()), panel.PanelType, panel.PanelTitle, panel.PanelText, panel.PanelImageUrl, panel.PanelUrlDestination));
        }

        public static void UpdateAboutPanel(AboutPanel panel)
        {
            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{panelUpdate(input:{{ id: {panel.PanelId}, title: \"{panel.PanelTitle}\", body: \"{panel.PanelText}\", imageURL: \"{(panel.PanelImageUrl == null ? "" : panel.PanelImageUrl.ToString())}\", imageLinkURL: \"{(panel.PanelUrlDestination == null ? "" : panel.PanelUrlDestination.ToString())}\" }}) {{ err {{ message }}}}}}"
            };
            
            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while updating panel: {res.Errors[0].Message}");
            }
        }

        public static void DeleteAboutPanel(int id)
        {
            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{panelDelete(input:{{ id: {id} }}) {{ err {{ message }}}}}}"
            };
            
            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while deleting panel: {res.Errors[0].Message}");
            }
        }
        
        public static void DeleteAboutPanel(AboutPanel panel)
        {
            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{panelDelete(input:{{ id: {panel.PanelId} }}) {{ err {{ message }}}}}}"
            };
            
            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while deliting panel: {res.Errors[0].Message}");
            }
        }

        public static void SetAboutPanelOrder(int[] panelIds)
        {
            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{panelDelete(input:{{ ids: {panelIds} }}) {{ err {{ message }}}}}}"
            };
            
            GraphQLResponse res = Task.Run(() => Dlive.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while updating panel order: {res.Errors[0].Message}");
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

            if (res.Errors != null)
            {
                throw new Exception($"An error occured while updating panel order: {res.Errors[0].Message}");
            }
        }
    }
}
