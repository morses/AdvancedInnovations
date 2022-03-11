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

        public virtual DbSet<DiscordUser> DiscordUsers { get; set; } = null!;
        public virtual DbSet<Presence> Presences { get; set; } = null!;
        public virtual DbSet<Server> Servers { get; set; } = null!;
        public virtual DbSet<ServerPresenceJoin> ServerPresenceJoins { get; set; } = null!;
        public virtual DbSet<ServerUserJoin> ServerUserJoins { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DiscordDataConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiscordUser>(entity =>
            {
                entity.HasKey(e => e.DiscordUserPk)
                    .HasName("PK__DiscordU__1F12BE95DE672169");
            });

            modelBuilder.Entity<Presence>(entity =>
            {
                entity.HasKey(e => e.PresencePk)
                    .HasName("PK__Presence__4981B3D9C361AA75");
            });

            modelBuilder.Entity<Server>(entity =>
            {
                entity.HasKey(e => e.ServerPk)
                    .HasName("PK__Server__C56B038642CA3FA0");
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
