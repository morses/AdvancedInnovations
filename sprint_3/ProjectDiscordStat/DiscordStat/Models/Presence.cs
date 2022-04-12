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
        [StringLength(256)]
        public string? Id { get; set; }
        [Column("applicationID")]
        [StringLength(256)]
        public string? ApplicationId { get; set; }
        [StringLength(256)]
        public string? Name { get; set; }
        [StringLength(256)]
        public string? Details { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [StringLength(256)]
        public string? LargeImageId { get; set; }
        [StringLength(256)]
        public string? SmallImageId { get; set; }
        [StringLength(256)]
        public string? ServerId { get; set; }
        [StringLength(256)]
        public string? UserId { get; set; }
        [StringLength(256)]
        public string? Image { get; set; }

        [InverseProperty(nameof(ServerPresenceJoin.PresencePkNavigation))]
        public virtual ICollection<ServerPresenceJoin> ServerPresenceJoins { get; set; }
    }
}
