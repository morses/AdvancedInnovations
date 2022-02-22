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

        [Required]
        [Column("ID")]
        [StringLength(128)]
        public string Id { get; set; }
        [Key]
        public int ServerPk { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Owner { get; set; }
        [StringLength(50)]
        public string Icon { get; set; }
        [Required]
        [StringLength(50)]
        public string HasBot { get; set; }
        public int ApproximateMemberCount { get; set; }

        [InverseProperty(nameof(ServerUserJoin.ServerPkNavigation))]
        public virtual ICollection<ServerUserJoin> ServerUserJoins { get; set; }
    }
}
