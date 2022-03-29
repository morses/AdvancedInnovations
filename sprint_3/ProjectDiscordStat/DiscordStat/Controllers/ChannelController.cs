using Microsoft.AspNetCore.Mvc;
using DiscordStats.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;
using DiscordStats.DAL.Abstract;
using DiscordStats.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace DiscordStats.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    [Authorize]

    public class ChannelController : Controller
    {
        private readonly ILogger<ChannelController> _logger;
        private readonly IChannelRepository _channelRepository;
        private readonly IDiscordServicesForChannels _discordServicesForChannels;

        public ChannelController(ILogger<ChannelController> logger, IChannelRepository channelRepository, IDiscordServicesForChannels discordServicesForChannels)
        {
            _logger = logger;
            _channelRepository = channelRepository;
            _discordServicesForChannels = discordServicesForChannels;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostChannels(Channel[] channels)
        {
            var itWorked = await _discordServicesForChannels.ChannelEntryAndUpdateDbCheck(channels);

            return Json(itWorked);
        }

    }
}
