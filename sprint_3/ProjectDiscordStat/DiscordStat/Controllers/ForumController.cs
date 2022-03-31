using DiscordStats.Models;
using DiscordStats.DAL.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using DiscordStats.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using DiscordStats.ViewModel;

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

        public IActionResult Index()
        {
            IEnumerable<Server> servers = _serverRepository.GetAll();
            return View(servers);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Discord")]
        public IActionResult Forum()
        {
            //RetrieveItemFromDbForForumVM getAllServersFromDb = new(_serverRepository);
            //var servers = getAllServersFromDb.RetrieveServers(_serverRepository);
            var servers = _serverRepository.GetAll();

            var selectList = new SelectList(
            servers.Where(m => m.Owner == "true").ToList().Select(s => new { Text = $"{s.Name}", Value = s.Id }),
            "Value", "Text");
            ViewData["Id"] = selectList;

            ViewData["Message"] = servers.Where(m => m.Owner == "true").ToList().Select(s => s.Message);

            return View();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Discord")]
        public IActionResult ServerOnForum([Bind("Id, Message")] Server server)
        {
            //var messageLength = server.Message.Length;
            if (server.Id != null)
            {             
                string onForum = "true";
                if(server.Message == null)
                {
                    server.Message = "null";
                }
                _serverRepository.UpdateOnServerWithForumInfo(server.Id, onForum, server.Message);
                return RedirectToAction("Index");

            }
            else
            {
                IEnumerable<Server>? servers = _serverRepository.GetAll();

                var selectList = new SelectList(
                servers.Where(m => m.Owner == "true").ToList().Select(s => new { Text = $"{s.Name}", Value = s.Id }),
                "Value", "Text");
                ViewData["ServerBroadcasting"] = selectList;
                return RedirectToAction("Forum");
            }
                     
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Discord")]
        public IActionResult ServerOffForum([Bind("Id, Message")] Server server)
        {
            if (server.Id != null)
            {
                string onForum = "false";
                server.Message = "null";
                _serverRepository.UpdateOnServerWithForumInfo(server.Id, onForum, server.Message);
                return RedirectToAction("Index");
            }
            else
            {
                IEnumerable<Server>? servers = _serverRepository.GetAll();

                var selectList = new SelectList(
                servers.Where(m => m.Owner == "true").ToList().Select(s => new { Text = $"{s.Name}", Value = s.Id }),
                "Value", "Text");
                ViewData["ServerBroadcasting"] = selectList;
                return RedirectToAction("Forum");
            }

        }
    }
}
