using DataAccess.Models;

using Microsoft.EntityFrameworkCore;


namespace DataAccess.Data;

public class CounterfeitKBContext : DbContext
{
    public CounterfeitKBContext()
    {
    }

    public CounterfeitKBContext(DbContextOptions<CounterfeitKBContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<Color> Colors { get; set; } = null!;
    public virtual DbSet<Counterfeit> Counterfeits { get; set; } = null!;
    public virtual DbSet<Shape> Shapes { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
        #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            optionsBuilder.UseSqlite("DataSource=CounterfeitKB.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Color>(entity =>
        {
            entity.ToTable("color");

            entity.HasIndex(e => e.Id, "IX_color_id")
                  .IsUnique();

            entity.HasIndex(e => e.Name, "IX_color_name")
                  .IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.BotLine).HasColumnName("botLine");

            entity.Property(e => e.Name).HasColumnName("name");

            entity.Property(e => e.UpLine).HasColumnName("upLine");
        });

        modelBuilder.Entity<Counterfeit>(entity =>
        {
            entity.ToTable("counterfeit");

            entity.HasIndex(e => e.Id, "IX_counterfeit_id")
                  .IsUnique();

            entity.HasIndex(e => e.Name, "IX_counterfeit_name")
                  .IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.BotLineSize).HasColumnName("botLineSize");

            entity.Property(e => e.ColorId).HasColumnName("colorId");

            entity.Property(e => e.Name).HasColumnName("name");

            entity.Property(e => e.ShapeId).HasColumnName("shapeId");

            entity.Property(e => e.UpLineSize).HasColumnName("upLineSize");

            entity.HasOne(d => d.Color)
                  .WithMany(p => p.Counterfeits)
                  .HasForeignKey(d => d.ColorId)
                  .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Shape)
                  .WithMany(p => p.Counterfeits)
                  .HasForeignKey(d => d.ShapeId)
                  .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Shape>(entity =>
        {
            entity.ToTable("shape");

            entity.HasIndex(e => e.Formula, "IX_shape_formula")
                  .IsUnique();

            entity.HasIndex(e => e.Id, "IX_shape_id")
                  .IsUnique();

            entity.HasIndex(e => e.Name, "IX_shape_name")
                  .IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Formula).HasColumnName("formula");

            entity.Property(e => e.Name).HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    private void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        //throw new NotImplementedException();
    }
}
