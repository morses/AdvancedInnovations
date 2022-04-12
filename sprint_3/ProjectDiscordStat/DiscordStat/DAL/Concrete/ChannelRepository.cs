using System.Collections.Generic;
using System.Linq;
using DiscordStats.DAL.Abstract;
using DiscordStats.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.DAL.Concrete
{
    public class ChannelRepository : Repository<Channel>, IChannelRepository
    {
        public ChannelRepository(DiscordDataDbContext ctx) : base(ctx)
        {
        }

        public async Task<bool> UpdateMessageCount(MessageInfo message)
        {
            Task.Delay(300).Wait();
            await Task.Run(() =>
            {
                var channelTruth = false;
                var tempChannel = new Channel();
                foreach (Channel channel in GetAll().ToList())
                {
                    if (channel.Id == message.ChannelId)
                    {
                        channelTruth = true;
                        tempChannel = channel;
                    }
                }
                if (channelTruth)
                {
                    if (tempChannel.Count == null)
                    {
                        tempChannel.Count = 0;
                    }
                    tempChannel.Count += 1;
                    AddOrUpdate(tempChannel);
                }

            });
            return true;
        }
    }
}
