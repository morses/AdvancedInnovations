using DiscordStats.DAL.Abstract;
using DiscordStats.Models;
using DiscordStats.ViewModel;
using DiscordStats.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;


namespace DiscordStats.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IDiscordService _discord;
        private readonly IConfiguration _configuration;
        private readonly IServerRepository _serverRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IVoiceChannelRepository _voiceChannelRepository;
        public AccountController(ILogger<HomeController> logger, IDiscordService discord, IConfiguration config, IServerRepository serverRepository, IChannelRepository channelRepository, IVoiceChannelRepository voiceChannelRepository)
        {
            _logger = logger;
            _discord = discord;
            _configuration = config;
            _serverRepository = serverRepository;
            _channelRepository = channelRepository;
            _voiceChannelRepository = voiceChannelRepository;
        }

        [Authorize (AuthenticationSchemes = "Discord")]
        public async Task<IActionResult> Account()
        {
            // Don't use the ViewBag!  Use a viewmodel instead.
            // The data in ClaimTypes can be mocked.  Will have to wait though for how to do that.
            ViewBag.id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            ViewBag.name = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            string bearerToken = User.Claims.First(c => c.Type == ClaimTypes.Role).Value;
            string botToken = _configuration["API:BotToken"];


            //var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            //var updatedOwner = await _discord.UpdateOwner(_configuration["API:BotToken"], "952358862059614218", userId);

            IEnumerable<Server>? servers = await _discord.GetCurrentUserGuilds(bearerToken);

            foreach (Server server in servers)
            {              
                string hasBot = await _discord.CheckForBot(botToken, server.Id);
                if (hasBot == "true")
                {
                    var serverWithMemCount = await _discord.GetFullGuild(botToken, server.Id);

                    _discord.ServerEntryDbCheck(serverWithMemCount, hasBot, server.Owner);
                }
            }

            var userInfo = await _discord.GetCurrentUserInfo(bearerToken);


            ViewBag.hash = userInfo.Avatar;


        // Now we can inject a mock IDiscordService that fakes this method.  That will allow us to test
        // anything __after__ getting this list of servers, i.e. any logic that we perform with this data from
        // here on.  There's nothing here now but there presumably will be.  If this method used a viewmodel
        // then we could test this action method a little more, but it doesn't.

        // Unfortunately it doesn't allow us to test the actual code within the GetCurrentUserGuilds method.
        // For that we must take the next step in refactoring.
        //var test = await _discord.UpdateOwner(botToken);
            return View(servers);
        }

        [Authorize(AuthenticationSchemes = "Discord")]
        public async Task<IActionResult> Servers()
        {
            ViewBag.id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            ViewBag.name = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            string bearerToken = User.Claims.First(c => c.Type == ClaimTypes.Role).Value;

            IEnumerable<Server>? servers = await _discord.GetCurrentUserGuilds(bearerToken);

            foreach (var s in servers.Where(m => m.Owner == "true"))
            {
                s.HasBot = await _discord.CheckForBot(_configuration["API:BotToken"], s.Id);
            }


            return View(servers.Where(m => m.Owner == "true").ToList());
        }

        [Authorize(AuthenticationSchemes = "Discord")]
        public async Task<IActionResult> ServerChannels(string? serverId)
        {
            string botToken = _configuration["API:BotToken"];
            var servers = _serverRepository.GetAll();
            var selectedServer = servers.Where(m => m.Id == serverId).FirstOrDefault();

            IList<Channel> channels = new List<Channel>();
            if (selectedServer != null)
            {
                if (selectedServer.HasBot == "true")
                {
                    channels = _channelRepository.GetAll().Where(x => x.GuildId == selectedServer.Id).ToList();

                    ViewBag.hasBot = "true";

                }
                else
                {
                    ViewBag.hasBot = "false";
                }
            }
            else
            {
                ViewBag.hasBot = "false";
            }

            return View(channels);
        }

        [HttpPost]
        public IActionResult ChangePrivacy(string privacyString)
        {
            var listPrivacyChanges = privacyString.Split(' ');
            string privacy = listPrivacyChanges[0];
            string serverId = listPrivacyChanges[1];
            _serverRepository.UpdatePrivacy(serverId, privacy);
            return RedirectToAction("Servers");
        }


        [AllowAnonymous]
        public IActionResult Logout()
        {
            foreach (var cookie in HttpContext.Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Details(string? name)
        {

            string bearerToken = User.Claims.First(c => c.Type == ClaimTypes.Role).Value;
            IEnumerable<Server>? servers = await _discord.GetCurrentUserGuilds(bearerToken);
            var SelectedServer = servers.Where(m => m.Name == name).FirstOrDefault();
            SelectedServer.HasBot = await _discord.CheckForBot(_configuration["API:BotToken"], SelectedServer.Id);
            var vm = new ServerOwnerViewModel();
            var test = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            if (SelectedServer.HasBot == "true")
            {
                vm = await _discord.GetFullGuild(_configuration["API:BotToken"], SelectedServer.Id);
                var ServerOwner = await _discord.GetUserInfoById(_configuration["API:BotToken"], vm.Owner_Id);
                vm.HasBot = SelectedServer.HasBot;
                vm.Owner = ServerOwner.Username;

                vm.users = await _discord.GetCurrentGuildUsers(_configuration["API:BotToken"], vm.Id);

            }
            else
            {
                vm.Icon = SelectedServer.Icon;
                vm.Name = SelectedServer.Name;
                vm.Id = SelectedServer.Id;
                vm.HasBot = "false";
            }

          
            return View(vm);
        }
        [Authorize(AuthenticationSchemes = "Discord")]
        public void LeaveServer(string ServerId)
        {
            string userid = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            string bearerToken = User.Claims.First(c => c.Type == ClaimTypes.Role).Value;
            _discord.RemoveUserServer(_configuration["API:BotToken"], ServerId, userid);
        }
        
        public async void KickUser(string ServerId,string user)
        {
            var users = await _discord.GetCurrentGuildUsers(_configuration["API:BotToken"], ServerId);
            
            string UserId = "";
            foreach (var u in users)
            {
                if (u.user.UserName == user)
                    UserId = u.user.Id;
            }

            await _discord.RemoveUserServer(_configuration["API:BotToken"], ServerId, UserId);
        }
        [Authorize(AuthenticationSchemes = "Discord")]

        public async Task<IActionResult> ServerForm()
        {

            return View();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Discord")]
        public async Task<IActionResult> ServerForm(CreateServerVM vm)
        {
            var newServer = await _discord.CreateServer(_configuration["API:BotToken"], vm);
            vm = JsonConvert.DeserializeObject<CreateServerVM>(newServer);

            vm.accessCode = await addUsertoGuild(vm.Id);

            return RedirectToAction("ServerCreateUpdateOwner", vm );
        }

        [Authorize(AuthenticationSchemes = "Discord")]
        public async Task<IActionResult> ServerCreateUpdateOwner(CreateServerVM vm)
        {
            return View(vm);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Discord")]
        public async void ServerCreateUpdateOwner(string ServerId)
        {
            var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var updatedOwner = await _discord.UpdateOwner(_configuration["API:BotToken"], ServerId, userId);
        }

        public async Task<string> addUsertoGuild(string serverId)
        {
            var getResponse = await _discord.FindChannels(_configuration["API:BotToken"], serverId);
            var channelList = getResponse.Split("},")[2];
            var channelId = channelList.Split(",")[0].Split(":")[1];
            channelId = channelId.Remove(0, 2);
            channelId = channelId.Remove(channelId.Length - 1, 1);


            var postResponse = await _discord.AddMemberToGuild(_configuration["API:BotToken"], channelId);
            string codeValue = "";

            codeValue = postResponse.Split(",")[0].Split(":")[1];
            codeValue = codeValue.Remove(0, 2);
            codeValue = codeValue.Remove(codeValue.Length - 1, 1);
            return codeValue;
        }
        [Authorize(AuthenticationSchemes = "Discord")]
        public async Task<IActionResult> Games(string ServerId)
        {
            List<GamesVM> games = new List<GamesVM>();
            var presence_list = _discord.GetPresencesForServer(ServerId).Result;
            foreach(var presence in presence_list)
            {
                var duplicate = false;
                foreach (var game in games)
                {
                    if (game.name == presence.Name)
                    {
                        game.UserCount++;
                        duplicate = true;
                    }
                }
                    {
                        GamesVM newGame = new GamesVM();
                        newGame.ServerId = ServerId;
                        newGame.name= presence.Name;
                        newGame.UserCount = 1;
                        newGame.GameImage = presence.Image;

                    if(newGame.GameImage == null)
                    {
                        var game = await _discord.GetJsonStringFromEndpointGames(newGame.name);
                        newGame.icon = game.icon;
                        newGame.id = game.id;
                    }
                        games.Add(newGame);
                    }               
            }
            return View(games);
        }
        [Authorize(AuthenticationSchemes = "Discord")]
        public async Task<IActionResult> GameDetails(string gameName, string serverId)
        {
            var voiceChannels = _voiceChannelRepository.GetAll().Where(v => v.GuildId == serverId).ToList();
            var distinctTimes = voiceChannels.DistinctBy(p => p.Time.Value.Hour).ToList();
            var graphData = new List<VoiceChannelGraph>();
           foreach (var dt in distinctTimes)
            {
                VoiceChannelGraph voiceChannelGraph = new VoiceChannelGraph();
                voiceChannelGraph.hour = dt.Time.Value.Hour;
                voiceChannelGraph.TotalmemberCount = 0;
                voiceChannelGraph.divider = 0;
                graphData.Add(voiceChannelGraph);
                foreach (var vc in voiceChannels)
                {
                    if(vc.Time.Value.Hour == dt.Time.Value.Hour)
                    {
                        graphData.Where(g => g.hour == vc.Time.Value.Hour).First().TotalmemberCount += vc.Count;
                        graphData.Where(g => g.hour == vc.Time.Value.Hour).First().divider++;
                            

                    }
                }
            }
           foreach(var data in graphData)
            {
                data.avgMemberCount = (double)data.TotalmemberCount / data.divider;
            }
                return View();
        }
        [HttpGet]
        public IActionResult GetVoiceChannelInfoFromDatabase(string ServerId)
        {
            var voiceChannels = _voiceChannelRepository.GetAll().Where(v => v.GuildId == ServerId).ToList();
            var distinctTimes = voiceChannels.DistinctBy(p => p.Time.Value.Hour).ToList();
            var graphData = new List<VoiceChannelGraph>();
            foreach (var dt in distinctTimes)
            {
                VoiceChannelGraph voiceChannelGraph = new VoiceChannelGraph();
                voiceChannelGraph.hour = dt.Time.Value.Hour;
                voiceChannelGraph.TotalmemberCount = 0;
                voiceChannelGraph.divider = 0;
                graphData.Add(voiceChannelGraph);
                foreach (var vc in voiceChannels)
                {
                    if (vc.Time.Value.Hour == dt.Time.Value.Hour)
                    {
                        graphData.Where(g => g.hour == vc.Time.Value.Hour).First().TotalmemberCount += vc.Count;
                        graphData.Where(g => g.hour == vc.Time.Value.Hour).First().divider++;


                    }
                }
            }
            foreach (var data in graphData)
            {
                data.avgMemberCount = (double)data.TotalmemberCount / data.divider;
            }
            return Json(graphData);
        }
    }
}

