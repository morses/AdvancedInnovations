using System.Collections.Generic;
using DiscordStats.Models;

namespace DiscordStats.DAL.Abstract
{
    public interface IPresenceRepository : IRepository<Presence>
    {
        //IEnumerable<Presence> GetPresences();

        List<Presence> GetPresences(string serverId);

        void UpdatePresence(string presenceId, string presenceName);


        Presence FindPresence(string presenceName);


    }
}
