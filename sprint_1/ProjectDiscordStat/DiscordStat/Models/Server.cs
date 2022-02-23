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
        //[Column("Approximate_Member_Count")]
        public int? Approximate_Member_Count { get; set; }

        [InverseProperty(nameof(ServerUserJoin.ServerPkNavigation))]
        public virtual ICollection<ServerUserJoin> ServerUserJoins { get; set; }
    }
}
