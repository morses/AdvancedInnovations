using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.Models
{
    [Table("ServerUserJoin")]
    public partial class ServerUserJoin
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("ServerID")]
        public int? ServerId { get; set; }
        [Column("UserID")]
        public int? UserId { get; set; }

        [ForeignKey(nameof(ServerId))]
        [InverseProperty("ServerUserJoins")]
        public virtual Server? Server { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("ServerUserJoins")]
        public virtual User? User { get; set; }
    }
}
