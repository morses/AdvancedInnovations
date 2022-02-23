using DiscordStats.DAL.Abstract;
using DiscordStats.Models;
using System.Net;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace DiscordStats.DAL.Concrete
{
    // It would be nice to use constructor injection here to inject the ability to get a web request
    // but that won't work in cases like this where we'd have to know the User and their bearer token
    // before creating the service to inject.  Instead we use method parameter injection.

    public class DiscordService : IDiscordService
    {
        // Use constructor injection to get the http client factory, which we'll use to get an http client
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServerRepository _serverRepository;

        private DiscordDataDbContext _db = new DiscordDataDbContext();

        public DiscordService(IHttpClientFactory httpClientFactory, IServerRepository serverRepository)
        {
            _serverRepository = serverRepository;   
            _httpClientFactory = httpClientFactory;
        }


        // This method wraps up the minimum functionality needed to make a request
        // to an external dependency.  It cannot be unit tested, but we do need to fake it in
        // order to test code that uses it, i.e. GetCurrentUserGuilds below
        public async Task<string> GetJsonStringFromEndpoint(string bearerToken, string uri)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri)
            {
                Headers =
                    {
                        { HeaderNames.Accept, "application/json" },
                        { HeaderNames.Authorization, "Bearer " + bearerToken},
                        { HeaderNames.UserAgent, "DiscordStat" }
                    }
            };
            HttpClient httpClient = _httpClientFactory.CreateClient();
            // Note this is the blocking version.  Would be better to use the Async version
            HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);
            // This is only a minimal version; make sure to cover all your bases here
            if (response.IsSuccessStatusCode)
            {
                // same here, this is blocking; use ReadAsStreamAsync instead
                string responseText = await response.Content.ReadAsStringAsync();
                return responseText;
            }
            else
            {
                // What to do if failure? Should throw specific exceptions that explain what happened
                throw new HttpRequestException();
            }
        }
        public async Task<string> GetJsonStringFromEndpointWithUserParam(string botToken, string uri)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri)
            {
                Headers =
                    {
                        { HeaderNames.Accept, "application/json" },
                        { HeaderNames.Authorization, "Bot " + botToken},
                        { HeaderNames.UserAgent, "DiscordStat" }
                    }
            };
            HttpClient httpClient = _httpClientFactory.CreateClient();
            // Note this is the blocking version.  Would be better to use the Async version
            HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);
            // This is only a minimal version; make sure to cover all your bases here
            if (response.IsSuccessStatusCode)
            {
                // same here, this is blocking; use ReadAsStreamAsync instead
                string responseText = await response.Content.ReadAsStringAsync();
                return responseText;
            }
            else
            {
                // What to do if failure? Should throw specific exceptions that explain what happened
                throw new HttpRequestException();
            }
        }

        public async Task<string> GetJsonStringFromEndpointForBot(string botToken, string uri)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri)
            {
                Headers =
                {
                    { HeaderNames.Accept, "application/json" },
                    { HeaderNames.Authorization, "Bot " + botToken },
                    { HeaderNames.UserAgent, "DiscordStat" }

                }
            };
            HttpClient httpClient = _httpClientFactory.CreateClient();
            // Note this is the blocking version.  Would be better to use the Async version
            HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);
            // This is only a minimal version; make sure to cover all your bases here
            if (response.IsSuccessStatusCode)
            {
                // same here, this is blocking; use ReadAsStreamAsync instead
                return "true";
            }
            else
            {
                // What to do if failure? Should throw specific exceptions that explain what happened
                return "false";
            }
        }

        public async Task<List<Server>?> GetCurrentUserGuilds(string bearerToken)
        {

            // Remember to handle errors here
            string response = await GetJsonStringFromEndpoint(bearerToken, $"https://discord.com/api/users/@me/guilds");
            // And here
            List<Server>? servers = JsonConvert.DeserializeObject<List<Server>>(response);
            return servers;
        }

        public async Task<User?> GetCurrentUserInfo(string bearerToken)
        {
            // Remember to handle errors here
            string response = await GetJsonStringFromEndpoint(bearerToken, "https://discord.com/api/users/@me");
            // And here
            User? userInfo = JsonConvert.DeserializeObject<User>(response);
            return userInfo;
        }

        public async Task<Server?> GetCurrentGuild(string botToken, string serverId)
        {
            string uri = "https://discord.com/api/guilds/" + serverId + "/preview";
            // Remember to handle errors here
            string response = await GetJsonStringFromEndpointWithUserParam(botToken, uri);
            // And here
            Server? server = JsonConvert.DeserializeObject<Server>(response);
            return server;
        }


        public async Task<string?> CheckForBot(string botToken, string serverId)
        {
            string uri = "https://discord.com/api/guilds/" + serverId;
            // Remember to handle errors here
            string response = await GetJsonStringFromEndpointForBot(botToken, uri);
            // And here

            return response;
        }   
        
        public async Task<string?> AddMemberToGuild(string botToken, string serverId, string userId)
        {
            string uri = "https://discord.com/api/guilds/" + serverId + "/members/" + userId;
            string response = await GetJsonStringFromEndpointWithUserParam(botToken, uri);
            return response;
        }

        public void ServerEntryDbCheck(Server server, string hasBot, string serverOwner)
        {
            var dbServers = _serverRepository.GetAll();
            if (dbServers.Count() == 0)
            {
                var servMemberCount = server.Approximate_Member_Count;
                _serverRepository.AddOrUpdate(new() { Id = server.Id, Name = server.Name, Owner = serverOwner, Icon = server.Icon, HasBot = hasBot, Approximate_Member_Count = servMemberCount });
            }
            else
            {
                foreach (var dbServer in dbServers)
                {
                    if (server.Id != dbServer.Id)
                    {
                        var servMemberCount = server.Approximate_Member_Count;
                        _serverRepository.AddOrUpdate(new() { Id = server.Id, Name = server.Name, Owner = serverOwner, Icon = server.Icon, HasBot = hasBot, Approximate_Member_Count = servMemberCount });
                    }
                }
            }

        }

    }
}