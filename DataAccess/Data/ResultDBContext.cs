using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DataAccess.Models;

namespace DataAccess.Data
{
    public partial class ResultDBContext : DbContext
    {
        public ResultDBContext()
        {
        }

        public ResultDBContext(DbContextOptions<ResultDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<OriginalPath> OriginalPaths { get; set; } = null!;
        public virtual DbSet<Result> Results { get; set; } = null!;
        public virtual DbSet<ResultPath> ResultPaths { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserType> UserTypes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlite("DataSource=ResultDB.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OriginalPath>(entity =>
            {
                entity.ToTable("originalPath");

                entity.HasIndex(e => e.Id, "IX_originalPath_id")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Path).HasColumnName("path");
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.ToTable("result");

                entity.HasIndex(e => e.Id, "IX_result_id")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.OrigPathId).HasColumnName("origPathId");

                entity.Property(e => e.ResPathId).HasColumnName("resPathId");

                entity.Property(e => e.AnRes).HasColumnName("anRes");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.OrigPath)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.OrigPathId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ResPath)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.ResPathId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ResultPath>(entity =>
            {
                entity.ToTable("resultPath");

                entity.HasIndex(e => e.Id, "IX_resultPath_id")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.InitId).HasColumnName("initId");

                entity.Property(e => e.Path).HasColumnName("path");

                entity.HasOne(d => d.Init)
                    .WithMany(p => p.ResultPaths)
                    .HasForeignKey(d => d.InitId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.Id, "IX_user_id")
                    .IsUnique();

                entity.HasIndex(e => e.Login, "IX_user_login")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Login).HasColumnName("login");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Password).HasColumnName("password");

                entity.Property(e => e.TypeId).HasColumnName("typeId");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.ToTable("userType");

                entity.HasIndex(e => e.Id, "IX_userType_id")
                    .IsUnique();

                entity.HasIndex(e => e.Name, "IX_userType_name")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
