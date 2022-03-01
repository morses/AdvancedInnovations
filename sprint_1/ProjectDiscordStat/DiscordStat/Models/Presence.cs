using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.Models
{
    [Table("Presence")]
    public partial class Presence
    {
        public Presence()
        {
            ServerPresenceJoins = new HashSet<ServerPresenceJoin>();
        }

        [Key]
        public int PresencePk { get; set; }
        [Column("ID")]
        [StringLength(128)]
        public string Id { get; set; } = null!;
        [Column("applicationID")]
        [StringLength(128)]
        public string ApplicationId { get; set; } = null!;
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [StringLength(256)]
        public string Details { get; set; } = null!;
        [StringLength(256)]
        public string CreatedAt { get; set; } = null!;
        [StringLength(256)]
        public string? LargeImageId { get; set; }
        [StringLength(256)]
        public string? SmallImageId { get; set; }
        [StringLength(128)]
        public string ServerId { get; set; } = null!;

        [InverseProperty(nameof(ServerPresenceJoin.PresencePkNavigation))]
        public virtual ICollection<ServerPresenceJoin> ServerPresenceJoins { get; set; }
    }
}
