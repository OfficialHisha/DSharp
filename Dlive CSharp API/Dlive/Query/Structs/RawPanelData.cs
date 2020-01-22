using Newtonsoft.Json.Linq;
using System;

namespace DSharp.Dlive.Query
{
    public struct RawPanelData
    {
        public int id;
        public AboutPanelType type;
        public string title;
        public string body;
        public Uri imageURL;
        public Uri imageLinkURL;

        public RawPanelData(JObject panel)
        {
            Enum.TryParse(panel["type"].ToString().ToUpper(), out AboutPanelType type);

            id = int.Parse(panel["id"].ToString());
            this.type = type;
            title = panel["title"].ToString();
            body = panel["body"].ToString();
            imageURL = string.IsNullOrWhiteSpace(panel["imageURL"].ToString()) ? null : new Uri(panel["imageURL"].ToString());
            imageLinkURL = string.IsNullOrWhiteSpace(panel["imageLinkURL"].ToString()) ? null : new Uri(panel["imageLinkURL"].ToString());

        }

        public AboutPanel ToAboutPanel()
        {
            return new AboutPanel(id, type, title, body, imageURL, imageLinkURL);
        }
    }
}