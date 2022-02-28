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
        public int? ServerPk { get; set; }
        public int? DiscordUserPk { get; set; }
    }
}
