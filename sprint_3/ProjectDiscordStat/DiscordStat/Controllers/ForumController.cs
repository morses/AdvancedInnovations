using DiscordStats.Models;
using DiscordStats.DAL.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using DiscordStats.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiscordStats.Controllers
{
    public class ForumController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDiscordService _discord;
        private readonly IConfiguration _configuration;
        private readonly IServerRepository _serverRepository;

        public ForumController(ILogger<HomeController> logger, IDiscordService discord, IConfiguration config, IServerRepository serverRepository)
        {
            _logger = logger;
            _discord = discord;
            _configuration = config;
            _serverRepository = serverRepository;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Discord")]
        public IActionResult Forum()
        {

            IEnumerable<Server>? servers = _serverRepository.GetAll();

            var selectList = new SelectList(
            servers.Where(m => m.Owner == "true").ToList().Select(s => new{Text = $"{s.Name}", Value = s.Id}),
            "Value", "Text");
            ViewData["Id"] = selectList;

            return View();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Discord")]
        public IActionResult Forum([Bind("Id")] ServerOwnerViewModel server)
        {
            //ModelState.Remove("Id,Name,Owner,HasBot,Icon,Owner_Id,Verification_Level,Description,Premium_Tier,Approximate_Member_Count,Approximate_Presence_Count");
            //if(ModelState.IsValid)
            if(server.Id != null)
            {
                ViewBag.Hello = "Hello";
            }
            else
            {
                IEnumerable<Server>? servers = _serverRepository.GetAll();

                var selectList = new SelectList(
                servers.Where(m => m.Owner == "true").ToList().Select(s => new { Text = $"{s.Name}", Value = s.Id }),
                "Value", "Text");
                ViewData["ServerBroadcasting"] = selectList;

            }
            
            return View();
        }
    }
}
