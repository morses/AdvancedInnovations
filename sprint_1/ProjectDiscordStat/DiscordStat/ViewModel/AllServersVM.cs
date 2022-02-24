using System.Collections.Generic;
using DiscordStats.Models;
using DiscordStats.DAL.Abstract;


namespace DiscordStats.ViewModel
{
    public class AllServersVM
    {
        private readonly IServerRepository _serverRepository;

        public AllServersVM(IServerRepository serverRepository)
        {
            _serverRepository = serverRepository;   
        }

        public IList<ServerPartial>? AllServerNameAndMemCountContainer()
        {
            var servers = _serverRepository.GetAll();
            List<ServerPartial> serverNameAndCountList = new();
            foreach(var server in servers)
            {
                serverNameAndCountList.Add(new ServerPartial { Id = server.Id, Name = server.Name, Approximate_Member_Count = server.Approximate_Member_Count });
            }

            return serverNameAndCountList;
        }
    }
}
