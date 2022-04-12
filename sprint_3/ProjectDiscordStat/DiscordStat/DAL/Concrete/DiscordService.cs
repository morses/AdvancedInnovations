using DiscordStats.DAL.Abstract;
using DiscordStats.Models;
using System.Net;

using DiscordStats.ViewModels;

using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

using System.Web.Helpers;
using DiscordStats.ViewModel;
using System.Diagnostics;


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
        private readonly IPresenceRepository _presenceRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IVoiceChannelRepository _voiceChannelRepository;


        private DiscordDataDbContext _db = new DiscordDataDbContext();

        public DiscordService(IHttpClientFactory httpClientFactory, IServerRepository serverRepository, IPresenceRepository presenceRepository, IChannelRepository channelRepository, IVoiceChannelRepository voiceChannelRepository)
        {
            _serverRepository = serverRepository;
            _httpClientFactory = httpClientFactory;
            _presenceRepository = presenceRepository;
            _channelRepository = channelRepository; 
            _voiceChannelRepository = voiceChannelRepository;

        }


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

        public async Task<string> GetJsonStringFromEndpointDelete(string botToken, string uri)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri)
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
        public async Task<string> PostToDiscordEndPoint(string botToken, string uri)
        {
            var bodyAsJSON = $"{{\"name\": \"Thisisatest\"}}";
            HttpContent body = new StringContent(bodyAsJSON);
            body.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Headers =
                {
                    { HeaderNames.Accept, "application/json" },
                    { HeaderNames.Authorization, "Bot " + botToken},
                    { HeaderNames.UserAgent, "DiscordStat" }
                },
                Content = body

            };
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);
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
        public async Task<string> PostToDiscordCreate(string botToken, string uri, CreateServerVM vm)
        {
            var bodyAsJSON = $"{{\"name\": \"{vm.name}\", \"region?\": \"{vm.region}\", \"verification_level?\": \"{vm.verification_level}\"}}";
            HttpContent body = new StringContent(bodyAsJSON);
            body.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Headers =
                {
                    { HeaderNames.Accept, "application/json" },
                    { HeaderNames.Authorization, "Bot " + botToken},
                    { HeaderNames.UserAgent, "DiscordStat" }
                },
                Content = body
            };
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);

            if (response.IsSuccessStatusCode)
            {
                string responseText = await response.Content.ReadAsStringAsync();
                return responseText;
            }
            else
            {
                throw new HttpRequestException();
            }
        }
        public async Task<string> PatchToDiscordEndPoint(string botToken, string uri, string userId)
        {
            var bodyAsJSON = $"{{\"owner_id\": \"{userId}\"}}";
            HttpContent body = new StringContent(bodyAsJSON);
            body.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri)
            {
                Headers =
                {
                    { HeaderNames.Accept, "application/json" },
                    { HeaderNames.Authorization, "Bot " + botToken},
                    { HeaderNames.UserAgent, "DiscordStat" }
                },
                Content = body
            };
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);
            if (response.IsSuccessStatusCode)
            {
                string responseText = await response.Content.ReadAsStringAsync();
                return responseText;
            }
            else
            {
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
            HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);
            if (response.IsSuccessStatusCode)
                return "true";
            else
                return "false";
        }

        public async Task<List<Server>?> GetCurrentUserGuilds(string bearerToken)
        {

            // Remember to handle errors here
            string response = await GetJsonStringFromEndpoint(bearerToken, $"https://discord.com/api/users/@me/guilds");
            // And here
            List<Server>? servers = JsonConvert.DeserializeObject<List<Server>>(response);
            return servers;
        }

        public async Task<DiscordUser?> GetCurrentUserInfo(string bearerToken)
        {
            // Remember to handle errors here
            string response = await GetJsonStringFromEndpoint(bearerToken, "https://discord.com/api/users/@me");
            // And here
            DiscordUser? userInfo = JsonConvert.DeserializeObject<DiscordUser>(response);
            return userInfo;
        }
        public async Task<List<GuildUsers>?> GetCurrentGuildUsers(string botToken, string serverId)
        {
            string uri = "https://discord.com/api/guilds/" + serverId + "/members?limit=1000";
            string response = await GetJsonStringFromEndpointWithUserParam(botToken, uri);
            List<GuildUsers>? userInfo = JsonConvert.DeserializeObject<List<GuildUsers>?>(response);
            return userInfo;
        }

        public async Task<DiscordUser?> GetUserInfoById(string botToken, string UserId)
        {
            string uri = "https://discord.com/api/users/" + UserId;
            string response = await GetJsonStringFromEndpointWithUserParam(botToken, uri);
            DiscordUser? userInfo = JsonConvert.DeserializeObject<DiscordUser>(response);
            return userInfo;
        }

        public async Task<ServerOwnerViewModel?> GetFullGuild(string botToken, string serverId)
        {
            string uri = "https://discord.com/api/guilds/" + serverId + "?with_counts=true";
            string response = await GetJsonStringFromEndpointWithUserParam(botToken, uri);
            ServerOwnerViewModel? server = JsonConvert.DeserializeObject<ServerOwnerViewModel>(response);
            return server;
        }

        public async Task<string?> CheckForBot(string botToken, string serverId)
        {
            string uri = "https://discord.com/api/guilds/" + serverId;
            string response = await GetJsonStringFromEndpointForBot(botToken, uri);
            return response;
        }

        public async Task<string?> AddMemberToGuild(string botToken, string channelId)
        {
            string uri = "https://discord.com/api/channels/" + channelId + "/invites";
            string response = await PostToDiscordEndPoint(botToken, uri);
            return response;
        }

        public async Task<string?> FindChannels(string botToken, string serverId)
        {
            string uri = "https://discord.com/api/guilds/" + serverId + "/channels";
            string response = await GetJsonStringFromEndpointWithUserParam(botToken, uri);
            return response;
        }
        public void ServerEntryDbCheck(ServerOwnerViewModel server, string hasBot, string serverOwner)

        {
            var dbServers = _serverRepository.GetAll();
            var duplicate = false;
            var serverDuplicate = new Server();
            if (server.Description == null)
            {
                server.Description = "null";
            }
            if (server.Icon == null)
            {
                server.Icon = "null";
            }
            if (server.Owner == null)
            {
                server.Owner = "null";
            }

            if (dbServers.Count() == 0)
            {
                var servMemberCount = server.Approximate_Member_Count;
                _serverRepository.AddOrUpdate(new() { Id = server.Id, Name = server.Name, Owner = serverOwner, Icon = server.Icon, HasBot = hasBot, ApproximateMemberCount = servMemberCount, OwnerId = server.Owner_Id, VerificationLevel = server.Verification_Level, Description = server.Description, PremiumTier = server.Premium_Tier, ApproximatePresenceCount = server.Approximate_Presence_Count, Privacy="private", OnForum="false", Message="null" });
                duplicate = true;

            }

            foreach (var dbServer in dbServers)
            {
                if (dbServer.Id == server.Id)
                {
                    duplicate = true;
                    serverDuplicate = dbServer;
                }
            }
            if (!duplicate)
            {
                var servMemberCount = server.Approximate_Member_Count;
                _serverRepository.AddOrUpdate(new() { Id = server.Id, Name = server.Name, Owner = serverOwner, Icon = server.Icon, HasBot = hasBot, ApproximateMemberCount = servMemberCount, OwnerId = server.Owner_Id, VerificationLevel = server.Verification_Level, Description = server.Description, PremiumTier = server.Premium_Tier, ApproximatePresenceCount = server.Approximate_Presence_Count, Privacy="private", OnForum="false", Message="null" });

            }
            if (duplicate)
            {
                serverDuplicate.ApproximateMemberCount = server.Approximate_Member_Count;
                _serverRepository.AddOrUpdate(serverDuplicate);
            }
        }


        public async Task<string?> PresenceEntryAndUpdateDbCheck(Presence[] presences)
        {
            foreach (var presence in presences)
            {
                Debug.Write(presence.Name);
                presence.CreatedAt = presence.CreatedAt?.ToLocalTime();

                Task.Delay(300).Wait();
                await Task.Run(() =>
                {
                    var duplicate = false;
                    var updatePresence = false;


                    var allPresences = _presenceRepository.GetAll().ToList();

                    foreach (var p in allPresences)
                    {
                        if (presence.ServerId == p.ServerId && presence.Name == p.Name && presence.UserId == p.UserId && presence.CreatedAt?.Hour == p.CreatedAt?.Hour && presence.CreatedAt?.Date == p.CreatedAt?.Date)
                        {
                            duplicate = true;
                        }
                    }
                    if (!duplicate)
                    {
                        _presenceRepository.AddOrUpdate(presence);
                    }
                });
            }
            return "It Worked";
        }


        public async Task<string?> VoiceChannelEntryAndUpdateDbCheck(VoiceChannel[] voiceChannels)
        {
            var allChannels =  _voiceChannelRepository.GetAll().ToList();
            
            foreach (var channel in voiceChannels)
            {
                var duplicate = false;

                Task.Delay(300).Wait();
                await Task.Run(() =>
                {
                    var similarchannels = allChannels.Where(c => c.Id == channel.Id).ToList();

                    foreach (var originalChannel in similarchannels)
                    {
                            if (channel.Count > originalChannel.Count && channel.Time.Value.Day == originalChannel.Time.Value.Day && channel.Time.Value.Hour == originalChannel.Time.Value.Hour)
                            {
                                originalChannel.Count = channel.Count;
                                duplicate = true;
                                _voiceChannelRepository.AddOrUpdate(originalChannel);
                            }
                            if(channel.Time.Value.Day == originalChannel.Time.Value.Day && channel.Time.Value.Hour == originalChannel.Time.Value.Hour && channel.Count == originalChannel.Count)
                            {
                                duplicate = true;
                            }                    
                    }
                    if (!duplicate)
                        _voiceChannelRepository.AddOrUpdate(channel);
                });
            }
            
            return "It Worked";
        }
        

        public async Task<string?> RemoveUserServer(string botToken, string serverId, string UserId)
        {
            string uri = "https://discord.com/api/guilds/" + serverId +"/members/" + UserId;
            string response = await GetJsonStringFromEndpointDelete(botToken, uri);
            return response;
        }
        public async Task<string?> CreateServer(string botToken, CreateServerVM vm)
        {
            string uri = "https://discord.com/api/guilds";
            string response = await PostToDiscordCreate(botToken, uri, vm);
            return response;
        }
        public async Task<string?> UpdateOwner(string botToken, string serverId, string currentUser)
        {
            string uri = "https://discord.com/api/guilds/" + serverId;
            string response = await PatchToDiscordEndPoint(botToken, uri, currentUser);
            return response;
        }
        public async Task<List<Presence>?> GetPresencesForServer(string serverId)
        {
            var presences = _presenceRepository.GetPresences(serverId);
            return presences;
        }
        public async Task<GamesVM> GetJsonStringFromEndpointGames(string gameName)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://discord.com/api/applications/detectable")
            {
                Headers =
                {
                    { HeaderNames.Accept, "application/json" }
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
                var converted = JsonConvert.DeserializeObject<List<GamesVM>>(responseText);
                return converted.Where(x => x.name.ToUpper() == gameName.ToUpper()).FirstOrDefault();
            }
            else
            {
                // What to do if failure? Should throw specific exceptions that explain what happened
                throw new HttpRequestException();
            }
        }
    }
}