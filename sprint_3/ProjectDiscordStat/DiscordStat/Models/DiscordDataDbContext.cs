using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DiscordStats.Models
{
    public partial class DiscordDataDbContext : DbContext
    {
        public DiscordDataDbContext()
        {
        }

        public DiscordDataDbContext(DbContextOptions<DiscordDataDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Channel> Channels { get; set; }
        public virtual DbSet<DiscordUser> DiscordUsers { get; set; }
        public virtual DbSet<Presence> Presences { get; set; }
        public virtual DbSet<Server> Servers { get; set; }
        public virtual DbSet<ServerChannelJoin> ServerChannelJoins { get; set; }
        public virtual DbSet<ServerPresenceJoin> ServerPresenceJoins { get; set; }
        public virtual DbSet<ServerUserJoin> ServerUserJoins { get; set; }
        public virtual DbSet<VoiceChannel> VoiceChannels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DiscordDataConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Channel>(entity =>
            {
                entity.HasKey(e => e.ChannelPk)
                    .HasName("PK__Channel__38C3B1260AE916F7");
            });

            modelBuilder.Entity<DiscordUser>(entity =>
            {
                entity.HasKey(e => e.DiscordUserPk)
                    .HasName("PK__DiscordU__1F12BE95418C1CAD");
            });

            modelBuilder.Entity<Presence>(entity =>
            {
                entity.HasKey(e => e.PresencePk)
                    .HasName("PK__Presence__4981B3D922A405AC");
            });

            modelBuilder.Entity<Server>(entity =>
            {
                entity.HasKey(e => e.ServerPk)
                    .HasName("PK__Server__C56B0386275191D4");
            });

            modelBuilder.Entity<ServerChannelJoin>(entity =>
            {
                entity.HasOne(d => d.ChannelPkNavigation)
                    .WithMany(p => p.ServerChannelJoins)
                    .HasForeignKey(d => d.ChannelPk)
                    .HasConstraintName("ServerChannelJoinChannelPk");

                entity.HasOne(d => d.ServerPkNavigation)
                    .WithMany(p => p.ServerChannelJoins)
                    .HasForeignKey(d => d.ServerPk)
                    .HasConstraintName("ServerChannelJoinServerPk");
            });

            modelBuilder.Entity<ServerPresenceJoin>(entity =>
            {
                entity.HasOne(d => d.PresencePkNavigation)
                    .WithMany(p => p.ServerPresenceJoins)
                    .HasForeignKey(d => d.PresencePk)
                    .HasConstraintName("ServerPresenceJoinPresencePk");

                entity.HasOne(d => d.ServerPkNavigation)
                    .WithMany(p => p.ServerPresenceJoins)
                    .HasForeignKey(d => d.ServerPk)
                    .HasConstraintName("ServerPresenceJoinServerPk");
            });

            modelBuilder.Entity<ServerUserJoin>(entity =>
            {
                entity.HasOne(d => d.DiscordUserPkNavigation)
                    .WithMany(p => p.ServerUserJoins)
                    .HasForeignKey(d => d.DiscordUserPk)
                    .HasConstraintName("ServerUserJoinDiscordUserPk");

                entity.HasOne(d => d.ServerPkNavigation)
                    .WithMany(p => p.ServerUserJoins)
                    .HasForeignKey(d => d.ServerPk)
                    .HasConstraintName("ServerUserJoinServerPk");
            });

            modelBuilder.Entity<VoiceChannel>(entity =>
            {
                entity.HasKey(e => e.VoiceChannelPk)
                    .HasName("PK__VoiceCha__004F00F9DAED8DE0");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
