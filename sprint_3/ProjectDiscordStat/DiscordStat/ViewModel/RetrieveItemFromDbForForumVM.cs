//using System.Collections.Generic;
//using DiscordStats.DAL.Abstract;
//using DiscordStats.Models;

//namespace DiscordStats.ViewModel
//{
//    public class RetrieveItemFromDbForForumVM
//    {
//        private readonly IServerRepository _serverRepository;

//        public string Id { get; set; }

//        public RetrieveItemFromDbForForumVM(IServerRepository serverRepository)
//        {
//            _serverRepository = serverRepository;
//        }
//        public IEnumerable<Server> RetrieveServers(IServerRepository _serverRepository)
//        {
//            IEnumerable<Server> servers = _serverRepository.GetAll();

//            return servers;
//        }

//    }
//}
