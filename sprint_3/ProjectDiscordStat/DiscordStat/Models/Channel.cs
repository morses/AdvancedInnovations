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
        public Channel()
        {
            ChannelWebhookJoins = new HashSet<ChannelWebhookJoin>();
            ServerChannelJoins = new HashSet<ServerChannelJoin>();
        }

        [Key]
        public int ChannelPk { get; set; }
        [Column("ID")]
        [StringLength(256)]
        public string? Id { get; set; }
        [StringLength(256)]
        public string? Type { get; set; }
        [StringLength(256)]
        public string? Name { get; set; }
        public int? Count { get; set; }
        [Column("Guild_id")]
        [StringLength(256)]
        public string? GuildId { get; set; }

        [InverseProperty(nameof(ChannelWebhookJoin.ChannelPkNavigation))]
        public virtual ICollection<ChannelWebhookJoin> ChannelWebhookJoins { get; set; }
        [InverseProperty(nameof(ServerChannelJoin.ChannelPkNavigation))]
        public virtual ICollection<ServerChannelJoin> ServerChannelJoins { get; set; }
    }
}
