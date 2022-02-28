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
        [Column("ID")]
        [StringLength(128)]
        public string Id { get; set; } = null!;
        [Key]
        public int ServerPk { get; set; }
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [StringLength(50)]
        public string Owner { get; set; } = null!;
        [StringLength(256)]
        public string? Icon { get; set; }
        [StringLength(50)]
        public string HasBot { get; set; } = null!;
        [Column("Approximate_Member_Count")]
        public int? ApproximateMemberCount { get; set; }
        [Column("owner_id")]
        [StringLength(50)]
        public string OwnerId { get; set; } = null!;
        [Column("verification_level")]
        [StringLength(50)]
        public string VerificationLevel { get; set; } = null!;
        [Column("description")]
        [StringLength(256)]
        public string Description { get; set; } = null!;
        [Column("premium_tier")]
        [StringLength(50)]
        public string PremiumTier { get; set; } = null!;
        [Column("approximate_presence_count")]
        [StringLength(50)]
        public string ApproximatePresenceCount { get; set; } = null!;
    }
}
