using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public JObject livestream;
        public RawPanelData[] panels;
        public RawPrivateUserData @private;

        public RawUserData(JObject userData)
        {
            Enum.TryParse(userData["partnerStatus"].ToString().ToUpper(), out partnerStatus);

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
            if (userData.ContainsKey("panels"))
            {
                foreach (JObject panel in userData["panels"])
                {
                    panels.Add(new RawPanelData(panel));
                }
            }
            
            username = userData["username"].ToString();
            displayname = userData["displayname"].ToString();
            effect = userData.ContainsKey("effect") ? userData["effect"].ToString() : "";
            this.badges = badges.ToArray();
            deactivated = userData.ContainsKey("deactivated") ? bool.Parse(userData["deactivated"].ToString()) : false;
            avatar = string.IsNullOrWhiteSpace(userData["avatar"].ToString()) ? null : new Uri(userData["avatar"].ToString());
            followers = userData.ContainsKey("followers") ? userData["followers"] as JObject : null;
            treasureChest = userData.ContainsKey("treasureChest") ? userData["treasureChest"] as JObject : null;
            wallet = userData.ContainsKey("wallet") ? userData["wallet"] as JObject : null;
            livestream = userData.ContainsKey("livestream") ? userData["livestream"] as JObject : null;
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
                
            PublicUserData publicData = new PublicUserData(username, displayname, partnerStatus, livestream != null, effect, badges, deactivated, actualPanels.ToArray(), avatar, (long)followers["totalCount"],
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
            return new PublicUserData(username, displayname, partnerStatus, livestream != null, effect, badges, deactivated, actualPanels.ToArray(), avatar, (long)followers["totalCount"],
                (long)treasureChest["value"], (long)wallet["balance"], (long)wallet["totalEarning"]);
        }
    }
}