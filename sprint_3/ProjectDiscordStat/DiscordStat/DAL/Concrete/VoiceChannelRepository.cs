using System.Collections.Generic;
using System.Linq;
using DiscordStats.DAL.Abstract;
using DiscordStats.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.DAL.Concrete
{
    public class VoiceChannelRepository : Repository<VoiceChannel>, IVoiceChannelRepository
    {
        public VoiceChannelRepository(DiscordDataDbContext ctx) : base(ctx)
        {
        }
    }
}
