using System;
using System.Collections.Generic;

namespace DiscordStats.Models
{
    public partial class ServerUserJoin
    {
        public int Id { get; set; }
        public int? ServerId { get; set; }
        public int? UserId { get; set; }

        public virtual Server? Server { get; set; }
        public virtual User? User { get; set; }
    }
}
