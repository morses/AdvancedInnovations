using System.Collections.Generic;
using System.Linq;
using DiscordStats.DAL.Abstract;
using DiscordStats.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.DAL.Concrete
{
    public class ServerUserJoinRepository : Repository<ServerUserJoin>, IServerUserJoinRepository
    {
        public ServerUserJoinRepository(DiscordDataDbContext ctx) : base(ctx)
        {
        }

        public IEnumerable<ServerUserJoin> GetServerUserJoins()
        {
            List<ServerUserJoin> serverUserJoins = new List<ServerUserJoin>();
            //var servers = _dbSet; //.Select(a => a.Name).ToList();
            foreach (var s in _dbSet)
            {
                serverUserJoins.Add(s);
            }

            return serverUserJoins;

        }
    }
}
