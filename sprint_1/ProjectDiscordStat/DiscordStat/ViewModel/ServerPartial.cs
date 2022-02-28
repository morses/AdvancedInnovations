using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.Models
{ 
    public partial class ServerPartial
    {
        [StringLength(128)]
        public string Id { get; set; } = null!;

        [StringLength(50)]
        public string Name { get; set; } = null!;

        public int? Approximate_Member_Count { get; set; }

    }
}
