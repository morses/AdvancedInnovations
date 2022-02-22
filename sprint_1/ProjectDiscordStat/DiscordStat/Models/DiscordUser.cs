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
        public DiscordUser()
        {
            ServerUserJoins = new HashSet<ServerUserJoin>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [StringLength(256)]
        public string Servers { get; set; } = null!;
        [StringLength(50)]
        public string? Avatar { get; set; }

        [InverseProperty(nameof(ServerUserJoin.DiscordUser))]
        public virtual ICollection<ServerUserJoin> ServerUserJoins { get; set; }
    }
}
