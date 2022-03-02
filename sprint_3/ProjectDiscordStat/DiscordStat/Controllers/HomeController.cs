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


namespace DiscordStats.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServerRepository _serverRepository;
        private readonly IDiscordService _discord;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IServerRepository serverRepository, IDiscordService discord, IConfiguration config)
        {
            _logger = logger;
            _serverRepository = serverRepository;
            _discord = discord;
            _config = config;    
        }
        // Get: Home
        public IActionResult Index()
        {

            return View();
        }     

        public IActionResult Contact()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = "Discord")]
        public async Task<IActionResult> AllServers()
        {
            AllServersVM allServersNameAndMemCountVM = new (_serverRepository);

            return View(allServersNameAndMemCountVM);
        }
         
        [Authorize(AuthenticationSchemes = "Discord")]
        [HttpPost]
        public async Task<IActionResult> AddMemberToPickedServer(string? id)
        {
            string botToken = _config["API:BotToken"];
            string bearerToken = User.Claims.First(c => c.Type == ClaimTypes.Role).Value;
            var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var serverId = id;

            string? response = await _discord.AddMemberToGuild(botToken, serverId, userId, bearerToken);

            AddMemberToPickedServerVM addedMemberProcessInfoVM = new();
            //addedMemberProcessInfoVM.infoOfProcessOfBeingAdded = response;
            ViewBag.Response = addedMemberProcessInfoVM.infoOfProcessOfBeingAdded(response);

            return View();
        }

        [Authorize(AuthenticationSchemes = "Discord")]
        public IActionResult Privacy()
        {
            ViewBag.id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
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