using System.Collections.Generic;
using DiscordStats.Models;

namespace DiscordStats.DAL.Abstract
{
    public interface IChannelRepository : IRepository<Channel>
    {
        Task<bool> UpdateMessageCount(MessageInfo message);
    }
}
