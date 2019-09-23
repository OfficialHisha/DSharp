namespace DSharp
{
    public class AuthorizationException : DliveAPIException
    {
        public AuthorizationException(string message) : base(message) {}
    }
}