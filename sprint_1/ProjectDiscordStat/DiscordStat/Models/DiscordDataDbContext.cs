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
        public virtual DbSet<Server> Servers { get; set; } = null!;
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
                    .HasName("PK__DiscordU__1F12BE953B438C47");
            });

            modelBuilder.Entity<Server>(entity =>
            {
                entity.HasKey(e => e.ServerPk)
                    .HasName("PK__Server__C56B03867709F1D5");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
