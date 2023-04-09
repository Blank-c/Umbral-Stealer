namespace Umbral.payload.Browsers
{
    internal struct PasswordFormat
    {
        internal readonly string Username;

        internal readonly string Password;

        internal readonly string Url;

        internal PasswordFormat(string username, string password, string url)
        {
            Username = username;
            Password = password;
            Url = url;
        }
    }

    internal struct CookieFormat
    {
        internal string Host;

        internal string Name;

        internal string Path;

        internal string Cookie;

        internal ulong Expiry;

        internal CookieFormat(string host, string name, string path, string cookie, ulong expiry)
        {
            Host = host;
            Name = name;
            Path = path;
            Cookie = cookie;
            Expiry = expiry;
        }
    }
}