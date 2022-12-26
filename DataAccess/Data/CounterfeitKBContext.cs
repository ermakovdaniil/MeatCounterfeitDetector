using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DataAccess.Models;

namespace DataAccess.Data
{
    public partial class CounterfeitKBContext : DbContext
    {
        public CounterfeitKBContext()
        {
        }

        public CounterfeitKBContext(DbContextOptions<CounterfeitKBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Counterfeit> Counterfeits { get; set; } = null!;
        public virtual DbSet<CounterfeitPath> CounterfeitPaths { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlite("DataSource=CounterfeitKB.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Counterfeit>(entity =>
            {
                entity.ToTable("counterfeit");

                entity.HasIndex(e => e.Id, "IX_counterfeit_id")
                    .IsUnique();

                entity.HasIndex(e => e.Name, "IX_counterfeit_name")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<CounterfeitPath>(entity =>
            {
                entity.ToTable("counterfeitPath");

                entity.HasIndex(e => e.Id, "IX_counterfeitPath_id")
                    .IsUnique();

                entity.HasIndex(e => e.ImagePath, "IX_counterfeitPath_imagePath")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CounterfeitId).HasColumnName("counterfeitId");

                entity.Property(e => e.ImagePath).HasColumnName("imagePath");

                entity.HasOne(d => d.Counterfeit)
                    .WithMany(p => p.CounterfeitPaths)
                    .HasForeignKey(d => d.CounterfeitId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
