using System.Collections.Generic;
using System.Linq;
using DiscordStats.DAL.Abstract;
using DiscordStats.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.DAL.Concrete
{
    public class ServerRepository : Repository<Server>, IServerRepository
    {
        public ServerRepository(DbContext ctx) : base(ctx)
        {
        }


        public IEnumerable<Server> GetAll()
        {
            return GetAll().ToList();
        }
        //IEnumerable<Server> IServerRepository.GetAll()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
