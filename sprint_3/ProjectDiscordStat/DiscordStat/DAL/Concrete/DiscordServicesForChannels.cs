using DiscordStats.DAL.Abstract;
using DiscordStats.Models;

namespace DiscordStats.DAL.Concrete
{
    public class DiscordServicesForChannels : IDiscordServicesForChannels
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IChannelRepository _channelRepository;

        public DiscordServicesForChannels(IHttpClientFactory httpClientFactory, IChannelRepository channelRepository)
        {
            _httpClientFactory = httpClientFactory;
            _channelRepository = channelRepository;
        }

        public async Task<string?> ChannelEntryAndUpdateDbCheck(Channel[] channels)
        {
            foreach (var channel in channels)
            {
                var duplicate = false;

                Task.Delay(300).Wait();
                await Task.Run(() =>
                {
                    var allChannels = _channelRepository.GetAll().ToList();
                    var duplicateChannel = new Channel();
                    for (int i = 0; i < allChannels.Count(); i++)
                    {
                        if (channel.Id == allChannels[i].Id)
                        {
                            duplicate = true;
                            duplicateChannel = allChannels[i];
                        }
                    }
                    if (!duplicate)
                    {
                        _channelRepository.AddOrUpdate(channel);
                    }
                    if (duplicate)
                    {

                        _channelRepository.AddOrUpdate(duplicateChannel);
                    }
                });
            }
            return "It Worked";
        }
    }
}
