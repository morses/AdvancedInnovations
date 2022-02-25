using DiscordStats.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiscordStats.Controllers
{
    [Route("api/[action]")]
    [ApiController]

    public class ApiController : Controller
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<ApiController> _logger;

        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Guilds()
        {
            return Json(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray());
        }

        [HttpGet]
        public IActionResult Pizza()
        {
            return Json("p8izza");
        }

        [HttpPost]
        public IActionResult Post(Server Data)
        {
            //WeatherForecast wf = new WeatherForecast();
            //wf.Date = DateTime.Now;
            //wf.TemperatureC = 20;
            //wf.Summary = "Hot";
            //return Json(wf);

            //Server server = new Server();
            //server.Id = "testID";
            //server.Name = "testName";
            //server.Owner = "testOwner";
            //server.Icon = "testIcon";

            //var result = JsonConvert.DeserializeObject<WeatherForecast>(Data);
            Server server = Data;
            server.Name = "new name being sent through mvc";

            return Json(Data);

            //var result = JsonConvert.DeserializeObject<WeatherForecast>(Data);


            //return Json(Data);
        }

        [HttpPost]
        public IActionResult PostUsers(DiscordUser[] users)
        {
            foreach (var user in users)
            {
                Debug.Write(user.Id + user.Name + "\n");
            }
            return Json("It worked");
        }
    }
}
