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

namespace DiscordStats.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(AuthenticationSchemes = "Discord")]
        public IActionResult Privacy()
        {

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
        

        [Authorize(AuthenticationSchemes = "Discord")]
        public  IActionResult Account()
        {


            ViewBag.id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            ViewBag.name = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;

            var request = (HttpWebRequest)WebRequest.Create("https://discord.com/api/users/@me/guilds");
            request.Method = "GET";
            var t = User;
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + User.Claims.First(c => c.Type == ClaimTypes.Role).Value);
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
            var server = JsonConvert.DeserializeObject<List<Server>>(content);

            return View(server);
        }

    }
}