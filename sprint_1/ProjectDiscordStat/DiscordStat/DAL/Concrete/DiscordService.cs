using DiscordStats.DAL.Abstract;
using DiscordStats.Models;
using System.Net;
using Newtonsoft.Json;

namespace DiscordStats.DAL.Concrete
{
    // It would be nice to use constructor injection here to inject the ability to get a web request
    // but that won't work in cases like this where we'd have to know the User and their bearer token
    // before creating the service to inject.  Instead we use method parameter injection.

    public class DiscordService : IDiscordService
    {
        // This method wraps up the minimum functionality needed to make a request
        // to an external dependency.  It cannot be unit tested, but we do need to fake it in
        // order to test code that uses it, i.e. GetCurrentUserGuilds below
        public static string GetJsonStringFromEndpoint(string bearerToken, string uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + bearerToken);
            request.Headers.Add("Content-Type", "application/json");

            var content = string.Empty;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();
                    }
                }
            }
            return content;
        }

        // This is the method we can use from the controller, since the controller (usually) doesn't need to know
        // anything about how this information is acquired
        public List<Server>? GetCurrentUserGuilds(string bearerToken)
        {
            SendRequest sr = GetJsonStringFromEndpoint;
            return GetCurrentUserGuilds( bearerToken, sr);
        }

        // But this is the method that actually does the work AND can have fake data injected via the delegate method
        public List<Server>? GetCurrentUserGuilds(string bearerToken, SendRequest messageSender)
        {
            // Remember to handle errors here
            string response = messageSender(bearerToken, "https://discord.com/api/users/@me/guilds");
            // And here
            List<Server>? servers = JsonConvert.DeserializeObject<List<Server>>(response);
            return servers;
        }
    }
}