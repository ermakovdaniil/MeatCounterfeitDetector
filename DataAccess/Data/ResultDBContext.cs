using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data
{
    public partial class ResultDBContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ResultDBContext()
        {
        }

        public ResultDBContext(DbContextOptions<ResultDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<OriginalImage> OriginalImages { get; set; } = null!;
        public virtual DbSet<Result> Results { get; set; } = null!;
        public virtual DbSet<ResultImage> ResultImages { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
    }
}
