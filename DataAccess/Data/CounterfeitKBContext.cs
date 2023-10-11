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
}
