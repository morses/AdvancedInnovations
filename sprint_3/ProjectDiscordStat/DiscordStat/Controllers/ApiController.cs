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
using DiscordStats.ViewModels;

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
        private readonly IServerRepository _serverRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IMessageInfoRepository _messageInfoRepository;


        public ApiController(ILogger<ApiController> logger, IDiscordUserRepository discordUserRepo, IPresenceRepository presenceRepository, IDiscordService discord, IServerRepository serverRepository, IChannelRepository channelRepository, IMessageInfoRepository messageInfoRepository)
        {
            _logger = logger;
            _discordUserRepository = discordUserRepo;
            _presenceRepository = presenceRepository;
            _discord = discord;
            _serverRepository = serverRepository;
            _channelRepository = channelRepository;
            _messageInfoRepository = messageInfoRepository;
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

        public IActionResult GetPresenceDataFromDb()
        {
            _logger.LogInformation("GetPresenceDataFromDb");
            List<Presence> presences = _presenceRepository.GetAll().ToList(); // .Where(a => a. Privacy == "public").OrderByDescending(m => m.ApproximateMemberCount).Take(5);
            PresenceChartDataVM presenceChartDataVM = new();
            var presencesNameAndCount = presenceChartDataVM.AllPresenceNameListAndCount(presences);

            return Json(new { userPerGame = presencesNameAndCount });
        }





        [HttpPost]
        public async Task<IActionResult> PostServers(Server[] servers)
        {
            foreach (var server in servers)
            {
                var duplicate = false;

                Task.Delay(300).Wait();
                await Task.Run(() =>
                {
                    var allServers = _serverRepository.GetAll().ToList();
                    var duplicateServer = new Server();
                    for (int i = 0; i < allServers.Count(); i++)
                    {
                        if (server.Id == allServers[i].Id)
                        {
                            duplicate = true;
                            duplicateServer = allServers[i];
                        }
                    }
                    if (!duplicate)
                    {
                        _serverRepository.AddOrUpdate(server);
                    }
                    if (duplicate)
                    {
                        duplicateServer.Name = server.Name;
                        duplicateServer.Id = server.Id;
                        duplicateServer.ApproximateMemberCount = server.ApproximateMemberCount;
                        duplicateServer.ApproximatePresenceCount = server.ApproximatePresenceCount;
                        duplicateServer.Icon = server.Icon;
                        duplicateServer.HasBot = server.HasBot;
                        duplicateServer.OwnerId = server.OwnerId;
                        duplicateServer.PremiumTier = server.PremiumTier;
                        duplicateServer.VerificationLevel = server.VerificationLevel;
                        _serverRepository.AddOrUpdate(duplicateServer);
                    }
                });

            }
            return Json("It worked");
        }


        [HttpPost]
        public async Task<IActionResult> PostChannels(Channel[] channels)
        {
            var itWorked = await _discord.ChannelEntryAndUpdateDbCheck(channels);

            return Json(itWorked);
        }

        [HttpPost]
        public async Task<IActionResult> PostMessageData(MessageInfo message)
        {
            _messageInfoRepository.AddOrUpdate(message);
            await _channelRepository.UpdateMessageCount(message);
            return Json("It worked");
        }
    }
}