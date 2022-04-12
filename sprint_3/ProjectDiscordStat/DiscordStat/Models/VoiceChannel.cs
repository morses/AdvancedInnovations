using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.Models
{
    public partial class VoiceChannel
    {
        [Key]
        public int VoiceChannelPk { get; set; }
        [Column("ID")]
        [StringLength(256)]
        public string? Id { get; set; }
        [StringLength(256)]
        public string? Name { get; set; }
        public int? Count { get; set; }
        [Column("Guild_id")]
        [StringLength(256)]
        public string? GuildId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Time { get; set; }
    }
}
