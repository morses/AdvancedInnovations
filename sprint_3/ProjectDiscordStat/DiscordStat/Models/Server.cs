using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.Models
{
    [Table("Server")]
    public partial class Server
    {
        public Server()
        {
            ServerChannelJoins = new HashSet<ServerChannelJoin>();
            ServerPresenceJoins = new HashSet<ServerPresenceJoin>();
            ServerUserJoins = new HashSet<ServerUserJoin>();
        }

        [Column("ID")]
        [StringLength(128)]
        public string Id { get; set; } = null!;
        [Key]
        public int ServerPk { get; set; }
        [StringLength(256)]
        public string? Name { get; set; }
        [StringLength(256)]
        public string? Owner { get; set; }
        [StringLength(256)]
        public string? Icon { get; set; }
        [StringLength(256)]
        public string? HasBot { get; set; }
        [Column("Approximate_Member_Count")]
        public int? ApproximateMemberCount { get; set; }
        [Column("owner_id")]
        [StringLength(256)]
        public string? OwnerId { get; set; }
        [Column("verification_level")]
        [StringLength(256)]
        public string? VerificationLevel { get; set; }
        [Column("description")]
        [StringLength(256)]
        public string? Description { get; set; }
        [Column("premium_tier")]
        [StringLength(256)]
        public string? PremiumTier { get; set; }
        [Column("approximate_presence_count")]
        public int? ApproximatePresenceCount { get; set; }
        [StringLength(256)]
        public string? Privacy { get; set; }
        [StringLength(256)]
        public string? OnForum { get; set; }
        [StringLength(256)]
        public string? Message { get; set; }

        [InverseProperty(nameof(ServerChannelJoin.ServerPkNavigation))]
        public virtual ICollection<ServerChannelJoin> ServerChannelJoins { get; set; }
        [InverseProperty(nameof(ServerPresenceJoin.ServerPkNavigation))]
        public virtual ICollection<ServerPresenceJoin> ServerPresenceJoins { get; set; }
        [InverseProperty(nameof(ServerUserJoin.ServerPkNavigation))]
        public virtual ICollection<ServerUserJoin> ServerUserJoins { get; set; }
    }
}
