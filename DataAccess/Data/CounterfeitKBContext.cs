using DataAccess.Models;

using Microsoft.EntityFrameworkCore;


namespace DataAccess.Data;

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

    // TODO: скрыть
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=CounterfeitKB;Username=postgres;Password=sword-fish");
        }
    }
}
