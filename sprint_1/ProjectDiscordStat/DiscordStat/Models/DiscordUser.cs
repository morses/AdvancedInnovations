using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.Models
{
    [Table("DiscordUser")]
    public partial class DiscordUser
    {
        [Column("ID")]
        [StringLength(128)]
        public string Id { get; set; } = null!;
        [Key]
        public int DiscordUserPk { get; set; }
        [StringLength(50)]

        public string username { get; set; } = null!;

        [StringLength(256)]
        public string Servers { get; set; } = null!;
        [StringLength(256)]
        public string? Avatar { get; set; }
    }
}
