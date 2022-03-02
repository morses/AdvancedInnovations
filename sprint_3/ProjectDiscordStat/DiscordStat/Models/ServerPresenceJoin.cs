using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.Models
{
    [Table("ServerPresenceJoin")]
    public partial class ServerPresenceJoin
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        public int? ServerPk { get; set; }
        public int? PresencePk { get; set; }

        [ForeignKey(nameof(PresencePk))]
        [InverseProperty(nameof(Presence.ServerPresenceJoins))]
        public virtual Presence? PresencePkNavigation { get; set; }
        [ForeignKey(nameof(ServerPk))]
        [InverseProperty(nameof(Server.ServerPresenceJoins))]
        public virtual Server? ServerPkNavigation { get; set; }
    }
}
