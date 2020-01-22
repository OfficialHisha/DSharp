using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace DSharp.Dlive.Query
{
    public struct RawPrivateUserData
    {
        public JObject subscribers;
        public string email;
        public bool emailVerified;
        public string[] filterWords;
        public JObject streamKey;

        public RawPrivateUserData(JObject privateUserData)
        {
            List<string> filterWords = new List<string>();
            foreach (JObject filterWord in privateUserData["filterWords"])
            {
                filterWords.Add(filterWord.ToString());
            }

            subscribers = privateUserData["subscribers"] as JObject;
            email = privateUserData["email"].ToString();
            emailVerified = bool.Parse(privateUserData["emailVerified"].ToString());
            this.filterWords = filterWords.ToArray();
            streamKey = privateUserData["streamKey"] as JObject; ;
        }
    }
}