using System.Collections.Generic;
using System.Linq;
using DiscordStats.DAL.Abstract;
using DiscordStats.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.DAL.Concrete
{
    public class MessageInfoRepository : Repository<MessageInfo>, IMessageInfoRepository
    {
        public MessageInfoRepository(DiscordDataDbContext ctx) : base(ctx)
        {
        }
    }
}
