using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

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

        // TODO: скрыть
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ResultDB;Username=postgres;Password=sword-fish");
            }
        }
    }
}
