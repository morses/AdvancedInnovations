using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiscordStats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CurrentUserController : ControllerBase
    {
        [HttpGet("current")]
        public object Current()
        {
            return new
            {
                Status = "Ok",
                User = "Me"
            };
        }
    }
}
