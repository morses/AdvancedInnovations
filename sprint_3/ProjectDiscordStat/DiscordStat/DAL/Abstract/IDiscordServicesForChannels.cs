using DiscordStats.Models;
using DiscordStats.ViewModel;
using DiscordStats.ViewModels;

namespace DiscordStats.DAL.Abstract
{
    public interface IDiscordServicesForChannels
    {
        Task<string?> ChannelEntryAndUpdateDbCheck(Channel[] channel);
    }
}
