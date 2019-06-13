namespace Firepuma.Api.Common.Configure
{
    public class InfluxConfig
    {
        public readonly string Url;
        public readonly string Database;
        public readonly string Username;
        public readonly string Password;

        public InfluxConfig(string url, string database, string username, string password)
        {
            Url = url;
            Database = database;
            Username = username;
            Password = password;
        }
    }
}
