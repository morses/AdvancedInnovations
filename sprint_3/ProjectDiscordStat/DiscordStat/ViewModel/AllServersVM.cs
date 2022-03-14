using System.Collections.Generic;
using DiscordStats.Models;
using DiscordStats.DAL.Abstract;
using DiscordStats.ViewModels;

namespace DiscordStats.ViewModel
{
    public class AllServersVM
    {
        private readonly IServerRepository _serverRepository;


        public AllServersVM(IServerRepository serverRepository)
        {
            _serverRepository = serverRepository;

        }

        public IList<ServerOwnerViewModel>? AllServerNameAndMemCountContainer()
        {
            var servers = _serverRepository.GetAll().Where(s => s.Privacy == "public");
            List<ServerOwnerViewModel> serverNameAndCountList = new();
            foreach (var server in servers)
            {
                serverNameAndCountList.Add(new ServerOwnerViewModel { Id = server.Id, Name = server.Name, Approximate_Member_Count = server.ApproximateMemberCount });
            }

            return serverNameAndCountList.OrderByDescending(s => s.Approximate_Member_Count).Take(10).ToList();
        }
    }
}
