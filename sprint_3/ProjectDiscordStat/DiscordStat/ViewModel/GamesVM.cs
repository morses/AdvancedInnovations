using System.Collections.Generic;
using DiscordStats.Models;
using DiscordStats.DAL.Abstract;
using DiscordStats.ViewModels;

namespace DiscordStats.ViewModel
{
    public class GamesVM
    {
        public int UserCount { get; set; }
        public string name { get; set; }
        public string GameImage { get; set; }
        public string ServerId { get; set; }
        public string icon { get; set; }
        public string id { get; set; }
        public string smallImageId { get; set; }

    }
}
