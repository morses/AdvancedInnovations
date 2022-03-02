using System.Collections.Generic;
using System.Linq;
using DiscordStats.DAL.Abstract;
using DiscordStats.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.DAL.Concrete
{
    public class PresenceRepository : Repository<Presence>, IPresenceRepository
    {
        public PresenceRepository(DiscordDataDbContext ctx) : base(ctx)
        {
        }

        public IEnumerable<Presence> GetPresences()
        {
            List<Presence> presences = new List<Presence>();
            foreach (var s in _dbSet)
            {
                presences.Add(s);
            }

            return presences;

        }
    }
}