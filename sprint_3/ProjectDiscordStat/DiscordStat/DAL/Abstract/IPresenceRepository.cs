using System.Collections.Generic;
using DiscordStats.Models;

namespace DiscordStats.DAL.Abstract
{
    public interface IPresenceRepository : IRepository<Presence>
    {
        //IEnumerable<Presence> GetPresences();

        void UpdatePresence(string presenceId, string presenceName);

        Presence FindPresence(string presenceName);
    }
}
