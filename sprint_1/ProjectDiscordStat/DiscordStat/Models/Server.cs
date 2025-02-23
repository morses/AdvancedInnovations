﻿using System;
using System.Collections.Generic;

namespace DiscordStats.Models
{
    public partial class Server
    {
        public Server()
        {
            ServerUserJoins = new HashSet<ServerUserJoin>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Owner { get; set; } = null!;

        public virtual ICollection<ServerUserJoin> ServerUserJoins { get; set; }
    }
}
