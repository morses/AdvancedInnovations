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

        //public IEnumerable<Presence> GetPresences()
        //{
        //    List<Presence> presences = new List<Presence>();
        //    foreach (var s in _dbSet)
        //    {
        //        presences.Add(s);
        //    }

        //    return presences;

        //}

        public void UpdatePresence(string presenceServerId, string presenceName)
        {
            foreach (Presence presence in GetAll())
            {
                if(presence.ServerId == presenceServerId)
                {
                    presence.Name = presenceName;
                    AddOrUpdate(presence);
                }
            }
        }

        public Presence FindPresence(string presenceName)
        {
            Presence presence = null;
            foreach(var i in _dbSet)
            {
                if(i.Name == presenceName)
                {
                    presence = i;
                }
            }
            return presence;
        }


        public List<Presence> GetPresences(string serverId)
        {
            List<Presence> presences = _dbSet.Where(p => p.ServerId == serverId).ToList();
            return presences;
        }

    }
}