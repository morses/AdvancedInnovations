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

        public ApiController(ILogger<ApiController> logger, IDiscordUserRepository discordUserRepo, IPresenceRepository presenceRepository)
        {
            _logger = logger;
            _discordUserRepository = discordUserRepo;
            _presenceRepository = presenceRepository;
            //var _allDiscordUsers = new List<DiscordUser>();
            //foreach (var user in _discordUserRepository.GetAll())
            //{
            //    user.ToJson()
            //}
            //var _allPresences = JsonConvert.DeserializeObject<List<Presence>>(JsonConvert.SerializeObject(_presenceRepository.GetAll()));

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
            //var newPresence = new Presence()
            //{
            //    Id = "id",
            //    ApplicationId = "appId",
            //    Name = "name",
            //    Details = "details",
            //    CreatedAt = "created",
            //    LargeImageId = "largeimageid",
            //    SmallImageId = "smallimageid",
            //    ServerId = "serverid"
            //};

            //_discordUserRepository.AddOrUpdate(new()
            //{
            //    Id = "qwewsdfljnkas",
            //    Username = "qwewsqwsddfljnkas",
            //    Servers = "qwewsasfwge3dfljnkas",
            //    Avatar = "qwewsdsafawffljnkas",
            //});

            //_presenceRepository.AddOrUpdate(newPresence);
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

        //[HttpPost]
        //public async Task<IActionResult> PostUsers(DiscordUser[] users)
        //{
        //    Task.Delay(300).Wait();
        //    var allDiscordUsers = _discordUserRepository.GetAll().ToList();

        //    if (allDiscordUsers.Count() == 0)
        //    {
        //        _discordUserRepository.AddOrUpdate(new()
        //        {
        //            Id = users[0].Id,
        //            Username = users[0].Username,
        //            Servers = users[0].Servers,
        //            Avatar = users[0].Avatar,
        //        });

        //        allDiscordUsers.Add(users[0]);

        //    }

        //    foreach (var user in users)
        //    {
        //        //var allDiscordUsers = JsonConvert.DeserializeObject<List<DiscordUser>>(JsonConvert.SerializeObject(_discordUserRepository.GetAll()));

        //        Debug.Write(user.Id + user.Username + "\n");
        //        var duplicate = false;


        //        foreach (var discordUser in allDiscordUsers)
        //        {
        //            if (user.Id == discordUser.Id)
        //            {
        //                duplicate = true;
        //            }
        //        }

        //        if (!duplicate)
        //        {
        //            _discordUserRepository.AddOrUpdate(new()
        //            {
        //                Id = user.Id,
        //                Username = user.Username,
        //                Servers = user.Servers,
        //                Avatar = user.Avatar,
        //            });
        //        }
        //    }


        //    return Json("It worked");
        //}

        //[HttpPost]
        //public async Task<IActionResult> PostUsers(DiscordUser[] users)
        //{
        //    var newListUsers = new List<DiscordUser>();

        //    foreach (var user in users)
        //    {
        //        var duplicate = false;

        //        Task.Delay(500).Wait();
        //        await Task.Run(() =>
        //        {
        //            var allDiscordUsers = _discordUserRepository.GetAll();

        //            foreach (var user in allDiscordUsers)
        //            {
        //                if (user.Id == user.Id)
        //                {
        //                    duplicate = true;
        //                }
        //            }
        //            if (!duplicate)
        //            {
        //                newListUsers.Add(user);
        //            }
        //        });

        //    }
        //    foreach (var user in newListUsers)
        //    {
        //        _discordUserRepository.AddOrUpdate(user);
        //    }

        //    return Json("It worked");
        //}

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
        public IActionResult PostPresence(Presence[] presences)
        {
            //foreach (var presence in presences)
            //{
            //    Debug.Write("\n\n\n" + presence.Id + "\n");
            //    Debug.Write(presence.Name + "\n");
            //    Debug.Write(presence.CreatedAt + "\n");
            //    Debug.Write(presence.ApplicationId + "\n");
            //    Debug.Write(presence.SmallImageId + "\n");
            //    Debug.Write(presence.LargeImageId + "\n");
            //    Debug.Write(presence.Details + "\n");
            //    Debug.Write(presence.ServerId + "\n\n\n");
            //}

            if (presences.Count() >= 1)
            {
                var allPresences = _presenceRepository.GetAll().ToList();

                if (allPresences.Count() == 0)
                {
                    _presenceRepository.AddOrUpdate(new()
                    {
                        Id = presences[0].Id,
                        ApplicationId = presences[0].ApplicationId,
                        Name = presences[0].Name,
                        Details = presences[0].Details,
                        CreatedAt = presences[0].CreatedAt,
                        LargeImageId = presences[0].LargeImageId,
                        SmallImageId = presences[0].SmallImageId,
                        ServerId = presences[0].ServerId
                    });

                    allPresences.Add(presences[0]);

                }

                foreach (var presence in presences)
                {
                    if (presence.SmallImageId == null)
                    {
                        presence.SmallImageId = "null";
                    }
                    if (presence.LargeImageId == null)
                    {
                        presence.LargeImageId = "null";
                    }
                    var duplicate = false;

                    foreach (var activity in allPresences)
                    {
                        if (presence.Id == activity.Id)
                        {
                            duplicate = true;
                        }
                    }
                    if (!duplicate)
                    {
                        //_presenceRepository.AddOrUpdate(new()
                        //{
                        //    Id = presence.Id,
                        //    ApplicationId = presence.ApplicationId,
                        //    Name = presence.Name,
                        //    Details = presence.Details,
                        //    CreatedAt = presence.CreatedAt,
                        //    LargeImageId = presence.LargeImageId,
                        //    SmallImageId = presence.SmallImageId,
                        //    ServerId = presence.ServerId
                        //});

                        _presenceRepository.AddOrUpdate(new()
                        {
                            Id = "null",
                            ApplicationId = "null",
                            Name = "null",
                            Details = "null",
                            CreatedAt = "null",
                            LargeImageId = "null",
                            SmallImageId = "null",
                            ServerId = "null"
                        });
                        var test = _presenceRepository.GetAll();
                    }

                }
            }

            return Json("It worked");
        }
    }
}
