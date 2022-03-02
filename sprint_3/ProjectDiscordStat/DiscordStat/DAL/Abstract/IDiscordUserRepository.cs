using System.Collections.Generic;
using DiscordStats.Models;

namespace DiscordStats.DAL.Abstract
{
    public interface IDiscordUserRepository : IRepository<DiscordUser>
    {
        IEnumerable<DiscordUser> GetDiscordUsers();

    }
}
