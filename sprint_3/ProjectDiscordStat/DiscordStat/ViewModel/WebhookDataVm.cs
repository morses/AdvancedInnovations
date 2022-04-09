using DiscordStats.DAL.Abstract;
using DiscordStats.Models;
using DiscordStats.ViewModels;

namespace DiscordStats.ViewModel
{
    public class WebhookDataVm
    {
        private readonly IServerRepository _serverRepository;

        private readonly IChannelRepository _channelRepository;
        public WebhookDataVm(IServerRepository serverRepository, IChannelRepository channelRepository)
        {
            _serverRepository = serverRepository;
            _channelRepository=channelRepository;
        }

        public string? DataBeingSentBackForWebhook(WebhookUsageVM webhook)
        {
            string message = "";
            if (webhook.memberValue == true)
            {
                var servers = _serverRepository.GetAll();
                List<ServerOwnerViewModel> serverNameAndCountList = new();
                foreach (var server in servers)
                {
                    serverNameAndCountList.Add(new ServerOwnerViewModel { Id = server.Id, Name = server.Name, Approximate_Member_Count = server.ApproximateMemberCount });

                }
                var guild = serverNameAndCountList.Where(i => i.Id == webhook.guild_id).FirstOrDefault();
                message = "There are " + guild.Approximate_Member_Count.ToString() + " members in this server";
            }
            if (webhook.activityValue == true)
            {
                var channels = _channelRepository.GetAll();
                Channel channel = new();
                foreach(var c in channels)
                {
                    if(c.Id == webhook.channel_id)
                    {
                        channel = c;
                    }
                }
                if (channel.Count != null)
                {
                    message = "The message count in " + channel.Name + " is " + channel.Count.ToString();
                }
                else
                {
                    message = "The message count in " + channel.Name + " is 0";

                }
            }

            return message;
        }
    }
}
