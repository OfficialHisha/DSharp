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
        public string imageURL;
        public string imageLinkURL;

        public RawPanelData(JObject panel)
        {
            Enum.TryParse(panel["type"].ToString().ToUpper(), out AboutPanelType type);

            id = int.Parse(panel["id"].ToString());
            this.type = type;
            title = panel["title"].ToString();
            body = panel["body"].ToString();
            imageURL = panel["imageURL"].ToString();
            imageLinkURL = panel["imageLinkURL"].ToString();

        }

        public AboutPanel ToAboutPanel()
        {
            return new AboutPanel(id, type, title, body, string.IsNullOrWhiteSpace(imageURL.ToString()) ? null : Uri.TryCreate(imageURL.ToString(), UriKind.RelativeOrAbsolute, out Uri imgUri) ? imgUri : null, string.IsNullOrWhiteSpace(imageLinkURL.ToString()) ? null : Uri.TryCreate(imageLinkURL.ToString(), UriKind.RelativeOrAbsolute, out Uri imgLinkUri) ? imgLinkUri : null);
        }
    }
}