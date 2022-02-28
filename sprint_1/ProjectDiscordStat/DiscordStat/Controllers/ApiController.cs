using DiscordStats.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using RestSharp;
using System.Net.Http;
using System.Net.Http.Headers;
using Azure.Core;
using System.Net;
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


        private readonly IDiscordUserRepository _discordUserRepository;
        private readonly ILogger<ApiController> _logger;

        public ApiController(ILogger<ApiController> logger, IDiscordUserRepository discordUserRepo)
        {
            _logger = logger;
            _discordUserRepository = discordUserRepo;
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
                Debug.Write(user.Id + user.username + "\n");
                var duplicate = false;
                var allServers = _discordUserRepository.GetAll();

                foreach (var discordUser in allServers)
                {
                    if (user.Id == discordUser.Id)
                    {
                        duplicate = true;
                    }
                }
                if (!duplicate)
                {
                    _discordUserRepository.AddOrUpdate(user);
                }
            }
            return Json("It worked");
        }
    }
}
