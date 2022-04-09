using Microsoft.AspNetCore.Mvc;
using DiscordStats.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;
using DiscordStats.DAL.Abstract;
using DiscordStats.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace DiscordStats.Controllers
{
    [Authorize(AuthenticationSchemes = "Discord")]
    public class ChannelController : Controller
    {
        private readonly ILogger<ChannelController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IChannelRepository _channelRepository;
        private readonly IDiscordServicesForChannels _discordServicesForChannels;
        private readonly IServerRepository _serverRepository;

        public ChannelController(ILogger<ChannelController> logger, IConfiguration configuration, IChannelRepository channelRepository, IDiscordServicesForChannels discordServicesForChannels, IServerRepository serverRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _channelRepository = channelRepository;
            _serverRepository = serverRepository;
            _discordServicesForChannels = discordServicesForChannels;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("channel/[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> PostChannels(Channel[] channels)
        {
            var itWorked = await _discordServicesForChannels.ChannelEntryAndUpdateDbCheck(channels);

            return Json(itWorked);
        }

        public async Task<IActionResult> ServerChannels(string? serverId)
        {
            string botToken = _configuration["API:BotToken"];
            var servers = _serverRepository.GetAll();
            var selectedServer = servers.Where(m => m.Id == serverId).FirstOrDefault();

            IList<Channel> channels = new List<Channel>();
            if (selectedServer != null)
            {
                if (selectedServer.HasBot == "true")
                {
                    channels = _channelRepository.GetAll().Where(x => x.GuildId == selectedServer.Id).ToList();

                    ViewBag.hasBot = "true";

                }
                else
                {
                    ViewBag.hasBot = "false";
                }
            }
            else
            {
                ViewBag.hasBot = "false";
            }

            return View(channels);
        }

        public async Task<IActionResult> ChannelWebhooks(Channel channel)
        {
            string botToken = _configuration["API:BotToken"];
            string channelId = channel.Id;
            ViewBag.channel_id = channelId;
            IEnumerable<WebhookUsageVM> webhooks = await _discordServicesForChannels.GetChannelWebHooks(botToken, channelId);
            return View(webhooks);
        }

        public async Task<IActionResult> WebhookForm(string channelId)
        {
            ViewBag.channelId = channelId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WebhookForm(WebhookUsageVM vm)
        {
            string botToken = _configuration["API:BotToken"];
            var webhook = await _discordServicesForChannels.CreateWebhook(botToken, vm.channelId, vm.name);
            Webhook webhookObject = JsonConvert.DeserializeObject<Webhook>(webhook);
            return RedirectToAction("WebhookMessage", webhookObject);
        }

        public async Task<IActionResult> WebhookMessage(WebhookUsageVM webhook)
        {
            WebhookUsageVM vm = new WebhookUsageVM();
            vm.name = webhook.name;
            //vm.Id = webhook.Id; 
            vm.Token = webhook.Token;
            vm.guild_id = webhook.guild_id;
            vm.channel_id = webhook.channel_id;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> WebhookMessage(WebhookUsageVM webhook, string whatever)
        {
            string botToken = _configuration["API:BotToken"];
            WebhookDataVm vm = new WebhookDataVm(_serverRepository, _channelRepository);
            string messageData = vm.DataBeingSentBackForWebhook(webhook);
            await _discordServicesForChannels.SendMessageThroughWebhook(botToken, webhook.Id, webhook.Token, messageData);
            return View();
        }

        public async Task<IActionResult> DeleteWebhook(WebhookUsageVM webhook)
        {
            string botToken = _configuration["API:BotToken"];
            IEnumerable<Channel> channels = _channelRepository.GetAll();
            Channel channel = channels.Where( i => i.Id == webhook.channel_id).FirstOrDefault();
            await _discordServicesForChannels.DeleteWebhook(botToken, webhook.Id);
            return RedirectToAction("ChannelWebhooks", channel);
        }

    }
}
