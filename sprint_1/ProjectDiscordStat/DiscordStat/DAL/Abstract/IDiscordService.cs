using DiscordStats.Models;

namespace DiscordStats.DAL.Abstract
{
    public interface IDiscordService
    {
        public IEnumerable<Server>? GetCurrentUserGuilds(string bearerToken);
    }
}