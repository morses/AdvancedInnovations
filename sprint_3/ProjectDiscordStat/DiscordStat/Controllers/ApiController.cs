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
        private readonly IPresenceRepository _presenceRepository;
        private readonly ILogger<ApiController> _logger;
        private readonly IDiscordService _discord;


        public ApiController(ILogger<ApiController> logger, IDiscordUserRepository discordUserRepo, IPresenceRepository presenceRepository, IDiscordService discord)
        {
            _logger = logger;
            _discordUserRepository = discordUserRepo;
            _presenceRepository = presenceRepository;
            _discord = discord;
        }


        [HttpPost]
        public async Task<IActionResult> PostUsers(DiscordUser[] users)
        {
            foreach (var user in users)
            {
                var duplicate = false;

                Task.Delay(300).Wait();
                await Task.Run(() =>
                {
                    var allDiscordUsers = _discordUserRepository.GetAll().ToList();

                    for (int i = 0; i < allDiscordUsers.Count(); i++)
                    {
                        if (user.Id == allDiscordUsers[i].Id)
                        {
                            duplicate = true;
                        }
                    }
                    if (!duplicate)
                    {
                        _discordUserRepository.AddOrUpdate(user);
                    }
                });

            }
            return Json("It worked");
        }


        [HttpPost]
        public async Task<IActionResult> PostPresence(Presence[] presences)
        {
            
            var itWorked = await _discord.PresenceEntryAndUpdateDbCheck(presences);

            return Json(itWorked);
        }

        public IActionResult GetDataAsynchronousParallel()
        {
            _logger.LogInformation("GetDataAsynchronousParallel");
            List<Presence> presences = _presenceRepository.GetAll().ToList(); // .Where(a => a. Privacy == "public").OrderByDescending(m => m.ApproximateMemberCount).Take(5);
            PresenceChartDataVM presenceChartDataVM = new();
            var presencesNameAndCount = presenceChartDataVM.AllPresenceNameListAndCount(presences);

            return Json(new { userPerGame = presencesNameAndCount });
        }
    }
}