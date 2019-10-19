namespace DSharp.Dlive
{
    public struct PrivateUserData
    {
        public long SubscriberCount { get; }
        public string Email { get; }
        public string[] WordFilter { get; }
        public string StreamKey { get; }

        public PrivateUserData(long subscriberCount, string email, string[] wordFilter, string streamKey)
        {
            SubscriberCount = subscriberCount;
            Email = email;
            WordFilter = wordFilter;
            StreamKey = streamKey;
        }
    }
}