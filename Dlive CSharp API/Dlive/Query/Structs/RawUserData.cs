using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace DSharp.Dlive.Query
{
    public struct RawUserData
    {
        public string username;
        public string displayname;
        public PartnerStatus partnerStatus;
        public string effect;
        public Badge[] badges;
        public bool deactivated;
        public Uri avatar;
        public JObject followers;
        public JObject treasureChest;
        public JObject wallet;
        public RawPanelData[] panels;
        public RawPrivateUserData @private;

        public RawUserData(JObject userData)
        {
            Enum.TryParse(userData["partnerStatus"].ToString().ToUpper(), out PartnerStatus status);

            List<Badge> badges = new List<Badge>();
            if (userData.ContainsKey("badges"))
            {
                foreach (JObject badge in userData["badges"])
                {
                    Enum.TryParse(badge.ToString().ToUpper(), out Badge realBadge);
                    badges.Add(realBadge);
                }
            }

            List<RawPanelData> panels = new List<RawPanelData>();
            foreach (JObject panel in userData["panels"])
            {
                panels.Add(new RawPanelData(panel));
            }


            @private = new RawPrivateUserData();
            if (userData.ContainsKey("private"))
            {
                @private = new RawPrivateUserData(userData["private"] as JObject);
            }

            username = userData["username"].ToString();
            displayname = userData["displayname"].ToString();
            partnerStatus = status;
            effect = userData.ContainsKey("effect") ? userData["effect"].ToString() : "";
            this.badges = badges.ToArray();
            deactivated = bool.Parse(userData["deactivated"].ToString());
            avatar = string.IsNullOrWhiteSpace(userData["avatar"].ToString()) ? null : new Uri(userData["avatar"].ToString());
            followers = userData["followers"] as JObject;
            treasureChest = userData["treasureChest"] as JObject;
            wallet = userData["wallet"] as JObject;
            this.panels = panels.ToArray();
            @private = userData.ContainsKey("private") ? new RawPrivateUserData(userData["private"] as JObject) : new RawPrivateUserData();
    }

        public UserData ToUserData()
        {
            List<AboutPanel> actualPanels = new List<AboutPanel>();
            foreach (RawPanelData panel in panels)
            {
                actualPanels.Add(panel.ToAboutPanel());
            }
                
            PublicUserData publicData = new PublicUserData(username, displayname, partnerStatus, effect, badges, deactivated, actualPanels.ToArray(), avatar, (long)followers["totalCount"],
                (long)treasureChest["value"], (long)wallet["balance"], (long)wallet["totalEarning"]);
            PrivateUserData privateData = new PrivateUserData((long) @private.subscribers["totalCount"],
                @private.email, @private.filterWords, @private.streamKey["key"].ToString());
                
            return new UserData(publicData, privateData);
        }

        public PublicUserData ToPublicUserData()
        {
            List<AboutPanel> actualPanels = new List<AboutPanel>();
            foreach (RawPanelData panel in panels)
            {
                actualPanels.Add(panel.ToAboutPanel());
            }
            return new PublicUserData(username, displayname, partnerStatus, effect, badges, deactivated, actualPanels.ToArray(), avatar, (long)followers["totalCount"],
                (long)treasureChest["value"], (long)wallet["balance"], (long)wallet["totalEarning"]);
        }
    }
}