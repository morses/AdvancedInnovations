using DiscordStats.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using RestSharp;
using System.Net.Http;
using System.Net.Http.Headers;
using Azure.Core;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;
using DiscordStats.DAL.Abstract;
using DiscordStats.ViewModel;

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DiscordStats.ViewModels;

namespace DiscordStats.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServerRepository _serverRepository;
        private readonly IPresenceRepository _presenceRepository;
        private readonly IDiscordService _discord;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IServerRepository serverRepository, IPresenceRepository presenceRepository, IDiscordService discord, IConfiguration config)
        {
            _logger = logger;
            _serverRepository = serverRepository;
            _presenceRepository = presenceRepository;
            _discord = discord;
            _config = config;    
        }


        public IActionResult Index()
        {

            return View();
        }


        public IActionResult GetServerDataFromDb()
        {
            //_logger.LogInformation("GetServerDataFromDb");
            IList<Server> servers = _serverRepository.GetAll().Where(a => a.Privacy == "public").OrderByDescending(m => m.ApproximateMemberCount).Take(5).ToList();
            return Json(new { userPerServer = servers });
        }

        public IActionResult Contact()
        {
            return View();
        }

        public async Task<IActionResult> AllServers()
        {
            AllServersVM allServersNameMemCountAndPresenceInfoVM = new (_serverRepository, _presenceRepository);
            IList<ServerOwnerViewModel> listOfServerOwnerViewModel = allServersNameMemCountAndPresenceInfoVM.AllServerNameAndMemCountContainer();
            return View(listOfServerOwnerViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddMemberToPickedServer(string? id)
        {
            string botToken = _config["API:BotToken"];

            var getResponse = await _discord.FindChannels(botToken, id);
            var channelList = getResponse.Split("},")[2];
            var channelId = channelList.Split(",")[0].Split(":")[1];
            channelId = channelId.Remove(0, 2);
            channelId = channelId.Remove(channelId.Length - 1, 1);


            var postResponse = await _discord.AddMemberToGuild(botToken, channelId);
            //foreach ()
            string codeValue = "";

            codeValue = postResponse.Split(",")[0].Split(":")[1];
            codeValue = codeValue.Remove(0,2);
            codeValue = codeValue.Remove(codeValue.Length - 1, 1);

            string link = "https://discord.gg/" + codeValue;


            ViewBag.Response = link;

            return Redirect(link);
        }

        public IActionResult Privacy()
        {
            //ViewBag.id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult DiscordAuthFailed()
        {
            return View();
        }
 
    }
}