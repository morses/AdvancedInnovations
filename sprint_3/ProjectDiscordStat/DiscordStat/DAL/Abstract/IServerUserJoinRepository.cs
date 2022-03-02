using System.Collections.Generic;
using DiscordStats.Models;

namespace DiscordStats.DAL.Abstract
{
    public interface IServerUserJoinRepository : IRepository<ServerUserJoin>
    {
        IEnumerable<ServerUserJoin> GetServerUserJoins();

    }
}
