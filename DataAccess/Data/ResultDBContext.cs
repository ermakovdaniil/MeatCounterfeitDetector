using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
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
        public virtual DbSet<IdentityRole<Guid>> UserRoles { get; set; } = null!;
    }
}
