using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.Common.Request;
using GraphQL.Common.Response;

namespace DSharp.Dlive.Mutation
{
    public class Mutation
    {
        private DliveAccount _account;

        public Mutation(DliveAccount account)
        {
            _account = account;
        }

        public void SendChatMessage(string channelUsername, string message)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query =
                    $"mutation{{sendStreamchatMessage(input:{{ streamer: \"{channelUsername}\", message: \"{message}\", roomRole: Owner, subscribing: true}}) {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();
            
            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;
            
            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while sending chat message: {res.Errors[0].Message}");
            }
        }

        public void DeleteChatMessage(string channelUsername, string messageId)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{chatDelete(streamer: \"{channelUsername}\", id: \"{messageId}\") {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while deleting chat message: {res.Errors[0].Message}");
            }
        }

        public void AddModerator(string newModeratorUsername)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{moderatorUsername(username: \"{newModeratorUsername}\") {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while adding moderator: {res.Errors[0].Message}");
            }
        }

        public void RemoveModerator(string moderatorUsername)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{moderatorRemove(username: \"{moderatorUsername}\") {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while removing moderator: {res.Errors[0].Message}");
            }
        }

        public void BanUser(string channelUsername, string usernameToBan)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query =
                    $"mutation{{streamchatUserBan(streamer: \"{channelUsername}\", username: \"{usernameToBan}\") {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while banning user: {res.Errors[0].Message}");
            }
        }

        public void UnbanUser(string streamer, string banUser)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query =
                    $"mutation{{streamchatUserUnban(streamer: \"{streamer}\", username: \"{banUser}\") {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while unbanning user: {res.Errors[0].Message}");
            }
        }

        //TODO: Figure out granularity of duration (assuming seconds for now, but it could as well be minutes)
        public void TimeoutUser(string streamer, string timeoutUser, int seconds)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query =
                    $"mutation{{userTimeoutSet(streamer: \"{streamer}\", username: \"{timeoutUser}\", duration: {seconds}) {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while setting timeout for user: {res.Errors[0].Message}");
            }
        }

        public void SetChatMode(ChatMode newChatMode)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{chatModeSet(chatMode: {newChatMode}) {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while changing chatmode: {res.Errors[0].Message}");
            }
        }

        public void SetEmoteMode(bool disallowPersonalEmotes, bool disallowChannelEmotes,
            bool disallowGlobalEmotes)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query =
                    $"mutation{{emoteModeSet(emoteMode:{{ NoMineEmote: {disallowGlobalEmotes}, NoAllEmote: {disallowChannelEmotes}, NoGlobalEmote: {disallowGlobalEmotes}}}) {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while changing emoteMode: {res.Errors[0].Message}");
            }
        }

        public void SetChatCooldown(int cooldownSeconds)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{chatIntervalSet(seconds: {cooldownSeconds}) {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while changing chat cooldown: {res.Errors[0].Message}");
            }
        }

        public void AddFilterWord(string word)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{filterWordAdd(word: \"{word}\") {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while adding filtered word: {res.Errors[0].Message}");
            }
        }

        public void RemoveFilterWord(string word)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{filterWordDelete(word: \"{word}\") {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while removing filtered word: {res.Errors[0].Message}");
            }
        }

        public void BanEmote(string streamer, string emoteString)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query =
                    $"mutation{{emoteBan(streamer: \"{streamer}\", emoteStr: \"{emoteString}\") {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while banning emote: {res.Errors[0].Message}");
            }
        }

        public void UnbanEmote(string streamer, string emoteString)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query =
                    $"mutation{{emoteUnban(streamer: \"{streamer}\", emoteStr: \"{emoteString}\") {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while unbanning emote: {res.Errors[0].Message}");
            }
        }

        public void SetSubSettings(string badgeText, string badgeColor, string textColor, string[] benefits)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query =
                    $"mutation{{subSettingSet(subSetting:{{ badgeText: \"{badgeText}\", badgeColor: \"{badgeColor}\", textColor: \"{textColor}\", benefits: {benefits} }}) {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while setting sub settings: {res.Errors[0].Message}");
            }
        }

        public void OpenChest()
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{giveawayStart() {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while opening chest: {res.Errors[0].Message}");
            }
        }

        public void ClaimChest(string streamer)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{giveawayClaim(streamer: \"{streamer}\") {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while claiming chest: {res.Errors[0].Message}");
            }
        }

        public void Follow(string streamer)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{follow(streamer: \"{streamer}\") {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while following user: {res.Errors[0].Message}");
            }
        }

        public void Unfollow(string streamer)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{unfollow(streamer: \"{streamer}\") {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while unfollowing user: {res.Errors[0].Message}");
            }
        }

        public void AddAboutPanel(string title = "New Panel", string content = "No content added",
            Uri image = null, Uri imageDestination = null, AboutPanelType type = AboutPanelType.DEFAULT)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{panelAdd(input:{{ type: {type} }}) {{ err {{ message }} panel {{ id }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while creating a new panel: {res.Errors[0].Message}");
            }

            UpdateAboutPanel(new AboutPanel(int.Parse(res.Data.panelAdd.panel.id.ToString()), type, title, content,
                image, imageDestination));
        }

        public void AddAboutPanel(AboutPanel panel)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query =
                    $"mutation{{panelAdd(input:{{ type: {panel.PanelType} }}) {{ err {{ message }} panel {{ id }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while creating a new panel: {res.Errors[0].Message}");
            }

            UpdateAboutPanel(new AboutPanel(int.Parse(res.Data.panel.id.ToString()), panel.PanelType, panel.PanelTitle,
                panel.PanelText, panel.PanelImageUrl, panel.PanelUrlDestination));
        }

        public void UpdateAboutPanel(AboutPanel panel)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query =
                    $"mutation{{panelUpdate(input:{{ id: {panel.PanelId}, title: \"{panel.PanelTitle}\", body: \"{panel.PanelText}\", imageURL: \"{(panel.PanelImageUrl == null ? "" : panel.PanelImageUrl.ToString())}\", imageLinkURL: \"{(panel.PanelUrlDestination == null ? "" : panel.PanelUrlDestination.ToString())}\" }}) {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while updating panel: {res.Errors[0].Message}");
            }
        }

        public void DeleteAboutPanel(int id)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{panelDelete(input:{{ id: {id} }}) {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while deleting panel: {res.Errors[0].Message}");
            }
        }

        public void DeleteAboutPanel(AboutPanel panel)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{panelDelete(input:{{ id: {panel.PanelId} }}) {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while deliting panel: {res.Errors[0].Message}");
            }
        }

        public void SetAboutPanelOrder(int[] panelIds)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{panelDelete(input:{{ ids: {panelIds} }}) {{ err {{ message }}}}}}"
            };

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while updating panel order: {res.Errors[0].Message}");
            }
        }

        public void SetAboutPanelOrder(AboutPanel[] panels)
        {
            if (!_account.IsAuthenticated)
                _account.RaiseError(
                    "Authentication is required to use mutations. Set the AuthorizationToken property with your user token to authenticate");

            List<int> ids = new List<int>();

            foreach (AboutPanel panel in panels)
            {
                ids.Add(panel.PanelId);
            }

            if (!Dlive.CanExecuteQuery())
                Task.Delay((Dlive.NextIntervalReset - DateTime.Now).Milliseconds).Wait();
            Dlive.IncreaseQueryCounter();

            GraphQLRequest _req = new GraphQLRequest
            {
                Query = $"mutation{{panelDelete(input:{{ ids: {ids.ToArray()} }}) {{ err {{ message }}}}}}"
            };

            GraphQLResponse res = Task.Run(() => _account.Client.SendMutationAsync(_req)).Result;

            if (res.Errors != null)
            {
                _account.RaiseError($"An error occured while updating panel order: {res.Errors[0].Message}");
            }
        }
    }
}