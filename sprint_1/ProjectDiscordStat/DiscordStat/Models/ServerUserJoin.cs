using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.Models
{
    [Table("ServerUserJoin")]
    public partial class ServerUserJoin
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        public int? ServerPk { get; set; }
        [Column("DiscordUserID")]
        public int? DiscordUserId { get; set; }

        [ForeignKey(nameof(DiscordUserId))]
        [InverseProperty("ServerUserJoins")]
        public virtual DiscordUser DiscordUser { get; set; }
        [ForeignKey(nameof(ServerPk))]
        [InverseProperty(nameof(Server.ServerUserJoins))]
        public virtual Server ServerPkNavigation { get; set; }
    }
}
