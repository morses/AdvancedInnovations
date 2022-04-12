using System.Collections.Generic;
using DiscordStats.Models;
using DiscordStats.DAL.Abstract;
using DiscordStats.ViewModels;

namespace DiscordStats.ViewModel
{
    public class AllServersVM
    {
        private readonly IServerRepository _serverRepository;
        private readonly IPresenceRepository _presenceRepository;
        List<ServerOwnerViewModel> serverNameCountAndPresenceList = new();
        string mostUsed = "";

        public AllServersVM(IServerRepository serverRepository, IPresenceRepository presenceRepository)
        {
            _serverRepository = serverRepository;
            _presenceRepository = presenceRepository;
        }

        public IList<ServerOwnerViewModel>? AllServerNameAndMemCountContainer()
        {
            var servers = _serverRepository.GetAll().Where(s => s.Privacy == "public");
            var presences = _presenceRepository.GetAll();

            List<Presence> presensesInServer = new();
            foreach (var server in servers)
            {
                foreach(var presence in presences)
                {
                    if(server.Id == presence.ServerId)
                    {

                       presensesInServer.Add(presence);
                       
                    }
                }
                if (!presensesInServer.Count().Equals(0))
                {
                    mostUsed = (from i in presensesInServer group i by i.Name into g orderby g.Count() descending select g.Key).First();
                }
                else
                {
                    mostUsed = "";
                }
                Presence presenceFromDb = _presenceRepository.FindPresence(mostUsed);
                serverNameCountAndPresenceList.Add(new ServerOwnerViewModel { Id = server.Id, Name = server.Name, Approximate_Member_Count = server.ApproximateMemberCount, presence = presenceFromDb });
            }

            return serverNameCountAndPresenceList.OrderByDescending(s => s.Approximate_Member_Count).Take(10).ToList();
        }


    }
}
