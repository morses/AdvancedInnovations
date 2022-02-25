using DiscordStats.Models;

namespace DiscordStats.DAL.Abstract
{
    //// This is how we will allow mocking of GetJsonStringFromEndpoint.  It is a delegate method
    //// See: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/delegates/
    //// It is like a function pointer in C++, or how you can "pass a function" to a method like
    //// we've been doing in LINQ expressions all along
    //public delegate string SendRequest(string bearerToken, string uri);

    public interface IDiscordService
    {
        /// <summary>
        /// Get all servers for the user with the given token and the default message sender.
        /// </summary>
        /// <param name="bearerToken"></param>
        /// <returns></returns>
        Task<List<Server>?> GetCurrentUserGuilds(string bearerToken);

        Task<User?> GetCurrentUserInfo(string bearerToken);

        Task<Server?> GetCurrentGuild(string botToken, string serverId);

        Task<string?> CheckForBot(string botToken, string serverId);

        Task<string?> AddMemberToGuild(string botToken, string serverId, string userId);

        void ServerEntryDbCheck(Server server, string hasBot, string serverOwner);
    }
}
