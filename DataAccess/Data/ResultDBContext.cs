using DataAccess.Models;

using Microsoft.EntityFrameworkCore;


namespace DataAccess.Data;

public partial class ResultDBContext : DbContext
{
    public ResultDBContext()
    {
    }

    public ResultDBContext(DbContextOptions<ResultDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Company> Companies { get; set; } = null!;
    public virtual DbSet<OriginalPath> OriginalPaths { get; set; } = null!;
    public virtual DbSet<Result> Results { get; set; } = null!;
    public virtual DbSet<ResultPath> ResultPaths { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
        #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            optionsBuilder.UseSqlite("DataSource=ResultDB.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("company");

            entity.HasIndex(e => e.Id, "IX_company_id")
                  .IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name).HasColumnName("name");
        });

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

            entity.Property(e => e.CompanyId).HasColumnName("companyId");

            entity.Property(e => e.Date).HasColumnName("date");

            entity.Property(e => e.OrigPathId).HasColumnName("origPathId");

            entity.Property(e => e.ResPathId).HasColumnName("resPathId");

            entity.Property(e => e.AnRes).HasColumnName("result");

            entity.HasOne(d => d.Company)
                  .WithMany(p => p.Results)
                  .HasForeignKey(d => d.CompanyId)
                  .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.OrigPath)
                  .WithMany(p => p.Results)
                  .HasForeignKey(d => d.OrigPathId)
                  .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ResPath)
                  .WithMany(p => p.Results)
                  .HasForeignKey(d => d.ResPathId)
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
