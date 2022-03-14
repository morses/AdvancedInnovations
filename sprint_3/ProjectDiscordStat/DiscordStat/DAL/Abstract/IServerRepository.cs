using System.Collections.Generic;
using DiscordStats.Models;


namespace DiscordStats.DAL.Abstract
{
    public interface IServerRepository : IRepository<Server>    
    {
        IEnumerable<Server> GetServers();
        bool CreateServer(Server server);
        bool CheckForDuplicates(string serverId);
        bool UpdatePrivacy(string serverId, string Privacy);

        void UpdateOnServerWithForumInfo(string serverId, string onForum, string message);

    }
}
