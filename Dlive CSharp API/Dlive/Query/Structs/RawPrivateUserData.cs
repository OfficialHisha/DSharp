using Newtonsoft.Json.Linq;

namespace DSharp.Dlive.Query
{
    public struct RawPrivateUserData
    {
        public JObject subscribers;
        public string email;
        public bool emailVerified;
        public string[] filterWords;
        public JObject streamKey;
    }
}