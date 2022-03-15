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

        public bool CreateServer(Server server)
        {
            if (server == null)
                return false;
            if (CheckForDuplicates(server.Id) == false)
                return false;
            AddOrUpdate(server);
            return true;
        }

        public bool CheckForDuplicates(string serverId)
        {
            IEnumerable<Server> servers = GetServers();
            foreach(Server server in servers)
            {
                if (serverId == server.Id)
                {
                    return true;
                }
            }
            return false;
        }

        public bool UpdatePrivacy(string serverId, string privacy)
        {
            if (privacy != "private" && privacy != "public")
                return false;

            foreach (Server server in GetAll().ToList())
            {
                if (server.Id == serverId)
                {
                    server.Privacy = privacy;
                    AddOrUpdate(server);
                    return true;
                }
            }
            return false;
        }

        public void UpdateOnServerWithForumInfo(string serverId, string onForum, string message)
        {
            foreach(Server server in GetAll().ToList())
            {
                if(server.Id == serverId)
                {
                    server.OnForum = onForum;
                    server.Message = message;
                    AddOrUpdate(server);
                }
            }

        }
    }
}
