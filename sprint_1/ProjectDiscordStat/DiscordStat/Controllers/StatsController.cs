using Microsoft.AspNetCore.Mvc;

namespace DiscordStats.Controllers
{
    public class StatsController : Controller
    {
        public IActionResult ServerStats()
        {
            return View();
        }
    }
}
