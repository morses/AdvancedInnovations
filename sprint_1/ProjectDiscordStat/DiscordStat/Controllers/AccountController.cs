using System.Net;
using System.Security.Claims;
using DiscordStats.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DiscordStats.Controllers
{
    public class AccountController : Controller
    {
        [Authorize(AuthenticationSchemes = "Discord")]
        public IActionResult Account()
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

        [Authorize(AuthenticationSchemes = "Discord")]
        public IActionResult Servers()
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
           

            return View(server.Where(m => m.Owner == "true").ToList());
        }
    }

}
