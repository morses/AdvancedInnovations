using System;
using System.Collections.Generic;

namespace DiscordStats.Models
{
    public partial class User
    {
        public User()
        {
            ServerUserJoins = new HashSet<ServerUserJoin>();
        }

        public string Id { get; set; }
        public string Name { get; set; } = null!;
        public string Servers { get; set; } = null!;
        public string Avatar { get; set; }

        public virtual ICollection<ServerUserJoin> ServerUserJoins { get; set; }
    }
}
