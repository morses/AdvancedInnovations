using DiscordStats.Models;

namespace DiscordStats.DAL.Abstract
{
    public interface IDiscordService
    {
        /// <summary>
        /// Get all servers for the user with the given token and the default message sender.
        /// </summary>
        /// <param name="bearerToken"></param>
        /// <returns></returns>
        Task<List<Server>?> GetCurrentUserGuilds(string bearerToken);
    }
}