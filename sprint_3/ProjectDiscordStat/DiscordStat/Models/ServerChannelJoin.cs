using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.Models
{
    [Table("ServerChannelJoin")]
    public partial class ServerChannelJoin
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        public int? ServerPk { get; set; }
        public int? ChannelsPk { get; set; }

        [ForeignKey(nameof(ServerPk))]
        [InverseProperty(nameof(Server.ServerChannelJoins))]
        public virtual Server? ServerPkNavigation { get; set; }
    }
}
