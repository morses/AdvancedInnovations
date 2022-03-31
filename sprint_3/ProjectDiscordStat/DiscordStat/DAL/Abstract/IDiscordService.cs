using DiscordStats.Models;
using DiscordStats.ViewModel;
using DiscordStats.ViewModels;


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

        Task<DiscordUser?> GetCurrentUserInfo(string bearerToken);

        Task<List<Channel>?> GetGuildChannels(string botToken, string serverId);

        Task<DiscordUser?> GetUserInfoById(string bearerToken, string UserId);


        Task<string?> CheckForBot(string botToken, string serverId);

        Task<string?> AddMemberToGuild(string botToken, string channelId);

        Task<string?> FindChannels(string botToken, string serverId);


        Task<ServerOwnerViewModel?> GetFullGuild(string botToken, string serverId);


        Task<List<GuildUsers>?> GetCurrentGuildUsers(string botToken, string serverId);
        Task<string?> RemoveUserServer(string botToken, string serverId, string userId);
        Task<string?> CreateServer(string botToken, CreateServerVM vm);
        Task<string?> UpdateOwner(string botToken, string serverId, string currentUser);

        void ServerEntryDbCheck(ServerOwnerViewModel server, string hasBot, string serverOwner);


        Task<string?> PresenceEntryAndUpdateDbCheck(Presence[] presences);
        Task<string?> ChannelEntryAndUpdateDbCheck(Channel[] channel);


    }
}
