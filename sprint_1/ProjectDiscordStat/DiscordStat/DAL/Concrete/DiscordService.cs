using DiscordStats.DAL.Abstract;
using DiscordStats.Models;

using System.Net.Http;
using System.Net.Http.Headers;
using Azure.Core;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DiscordStats.DAL.Concrete
{
    public class DiscordService : IDiscordService
    {
        public IEnumerable<Server>? GetCurrentUserGuilds(string bearerToken)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://discord.com/api/users/@me/guilds");
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
            IEnumerable<Server>? servers = JsonConvert.DeserializeObject<List<Server>>(content);
            return servers;
        }
    }
}