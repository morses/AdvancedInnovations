using System.Collections.Generic;
using System.Linq;
using DiscordStats.DAL.Abstract;
using DiscordStats.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.DAL.Concrete
{
    public class DiscordUserRepository : Repository<DiscordUser>, IDiscordUserRepository
    {
        public DiscordUserRepository(DiscordDataDbContext ctx) : base(ctx)
        {
        }

        public IEnumerable<DiscordUser> GetDiscordUsers()
        {
            List<DiscordUser> discordusers = new List<DiscordUser>();
            foreach (var s in _dbSet)
            {
                discordusers.Add(s);
            }

            return discordusers;

        }
    }
}
