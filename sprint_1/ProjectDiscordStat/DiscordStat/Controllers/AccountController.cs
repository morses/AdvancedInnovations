using DiscordStats.Models;
using DiscordStats.DAL.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DiscordStats.Controllers
{
    public class AccountController : Controller
    {

        private readonly ILogger<AccountController> _logger;
        private readonly IDiscordService _discord;

        public AccountController(ILogger<AccountController> logger, IDiscordService discord)
        {
            _logger = logger;
            _discord = discord;
        }

        [Authorize(AuthenticationSchemes = "Discord")]
        public IActionResult Account()
        {
            // Don't use the ViewBag!  Use a viewmodel instead.
            // The data in ClaimTypes can be mocked.  Will have to wait though for how to do that.
            ViewBag.id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            ViewBag.name = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            string bearerToken = User.Claims.First(c => c.Type == ClaimTypes.Role).Value;

            IEnumerable<Server>? servers = _discord.GetCurrentUserGuilds(bearerToken);

            var userInfo = _discord.GetCurrentUserInfo(bearerToken);


            ViewBag.hash = userInfo.Avatar;

            // Now we can inject a mock IDiscordService that fakes this method.  That will allow us to test
            // anything __after__ getting this list of servers, i.e. any logic that we perform with this data from
            // here on.  There's nothing here now but there presumably will be.  If this method used a viewmodel
            // then we could test this action method a little more, but it doesn't.

            // Unfortunately it doesn't allow us to test the actual code within the GetCurrentUserGuilds method.
            // For that we must take the next step in refactoring.

            return View(servers);
        }

        [Authorize(AuthenticationSchemes = "Discord")]
        public IActionResult Servers()
        {
            ViewBag.id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            ViewBag.name = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            string bearerToken = User.Claims.First(c => c.Type == ClaimTypes.Role).Value;

            IEnumerable<Server>? servers = _discord.GetCurrentUserGuilds(bearerToken);

            return View(servers.Where(m => m.Owner == "true").ToList());
        }
    }

}
