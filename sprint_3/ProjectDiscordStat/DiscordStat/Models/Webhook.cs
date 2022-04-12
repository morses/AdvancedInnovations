using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiscordStats.Models
{
    [Table("Webhook")]
    public partial class Webhook
    {
        public Webhook()
        {
            ChannelWebhookJoins = new HashSet<ChannelWebhookJoin>();
        }

        [Key]
        public int WebhookPk { get; set; }
        [Column("ID")]
        [StringLength(256)]
        public string? Id { get; set; }
        [StringLength(256)]
        public string? Type { get; set; }
        [StringLength(256)]
        public string? Name { get; set; }
        [StringLength(256)]
        public string? Avatar { get; set; }
        [Column("Channel_id")]
        [StringLength(256)]
        public string? ChannelId { get; set; }
        [Column("Guild_id")]
        [StringLength(256)]
        public string? GuildId { get; set; }
        [Column("Application_id")]
        [StringLength(256)]
        public string? ApplicationId { get; set; }
        [StringLength(256)]
        public string? Token { get; set; }

        [InverseProperty(nameof(ChannelWebhookJoin.WebhookPkNavigation))]
        public virtual ICollection<ChannelWebhookJoin> ChannelWebhookJoins { get; set; }
    }
}
