using System.Collections.Generic;
using DiscordStats.Models;


namespace DiscordStats.DAL.Abstract
{
    public interface IServerRepository : IRepository<Server>    
    {
        IEnumerable<Server> GetServers();

    }
}
