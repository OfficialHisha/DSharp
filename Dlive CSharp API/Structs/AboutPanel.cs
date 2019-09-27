using System;

namespace DSharp
{
    public struct AboutPanel
    {
        public int PanelId { get; }
        public AboutPanelType PanelType { get; }
        public string PanelTitle { get; set; }
        public string PanelText { get; set; }
        public Uri PanelImageUrl { get; set; }
        public Uri PanelUrlDestination { get; set; }

        public AboutPanel(int id, AboutPanelType type, string title, string content, Uri imageUrl, Uri destination)
        {
            PanelId = id;
            PanelType = type;
            PanelTitle = title;
            PanelText = content;
            PanelImageUrl = imageUrl;
            PanelUrlDestination = destination;
        }
    }
}