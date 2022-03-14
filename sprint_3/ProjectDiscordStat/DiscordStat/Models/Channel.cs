using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.Models
{
    [Table("Channel")]
    public partial class Channel
    {
        [Key]
        public int ChannelsPk { get; set; }
        [Column("ID")]
        [StringLength(256)]
        public string Id { get; set; } = null!;
        public int Type { get; set; }
        [StringLength(256)]
        public string Name { get; set; } = null!;
        public int Position { get; set; }
        [Column("Parent_id")]
        [StringLength(256)]
        public string ParentId { get; set; } = null!;
        [Column("Guild_id")]
        [StringLength(256)]
        public string GuildId { get; set; } = null!;
        [Column("Permission_overwrites")]
        [StringLength(256)]
        public string? PermissionOverwrites { get; set; }
        [StringLength(50)]
        public string? Nsfw { get; set; }
    }
}
