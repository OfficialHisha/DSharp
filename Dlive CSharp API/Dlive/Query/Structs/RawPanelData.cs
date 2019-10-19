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

        public AboutPanel ToAboutPanel()
        {
            return new AboutPanel(id, type, title, body, imageURL, imageLinkURL);
        }
    }
}