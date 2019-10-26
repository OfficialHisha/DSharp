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
        public bool deactivated;
        public Uri avatar;
        public JObject followers;
        public JObject treasureChest;
        public JObject wallet;
        public RawPanelData[] panels;
        public RawPrivateUserData @private;


        public UserData ToUserData()
        {
            List<AboutPanel> actualPanels = new List<AboutPanel>();
            foreach (RawPanelData panel in panels)
            {
                actualPanels.Add(panel.ToAboutPanel());
            }
                
            PublicUserData publicData = new PublicUserData(username, displayname, partnerStatus, effect, deactivated, actualPanels.ToArray(), avatar, (long)followers["totalCount"],
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
            return new PublicUserData(username, displayname, partnerStatus, effect, deactivated, actualPanels.ToArray(), avatar, (long)followers["totalCount"],
                (long)treasureChest["value"], (long)wallet["balance"], (long)wallet["totalEarning"]);
        }
    }
}