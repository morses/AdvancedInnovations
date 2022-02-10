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

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Servers { get; set; } = null!;

        public virtual ICollection<ServerUserJoin> ServerUserJoins { get; set; }
    }
}
