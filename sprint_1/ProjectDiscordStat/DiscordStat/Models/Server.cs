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
            ServerUserJoins = new HashSet<ServerUserJoin>();
        }

        [Key]
        [Column("ID")]
        public string Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [StringLength(50)]
        public string Owner { get; set; } = null!;
        [StringLength(256)]
        public string? Icon { get; set; }

        [InverseProperty(nameof(ServerUserJoin.Server))]
        public virtual ICollection<ServerUserJoin> ServerUserJoins { get; set; }
    }
}
