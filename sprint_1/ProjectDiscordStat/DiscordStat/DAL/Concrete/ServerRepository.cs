using System.Collections.Generic;
using System.Linq;
using DiscordStats.DAL.Abstract;
using DiscordStats.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.DAL.Concrete
{
    public class ServerRepository : Repository<Server>, IServerRepository
    {
        public ServerRepository(DiscordDataDbContext ctx) : base(ctx)
        {
        }

        public IEnumerable<Server> GetServers()
        {
            List<Server> servers = new List<Server>();
            //var servers = _dbSet; //.Select(a => a.Name).ToList();
            foreach (var s in _dbSet)
            {
                servers.Add(s);
            }

            return servers;

        }
    }
}
