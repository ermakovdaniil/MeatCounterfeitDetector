using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace MeatAPI.Features.User
{
    public class UserService : EntityAccessServiceBase<ResultDBContext, DataAccess.Models.User>
    {
        public UserService(ResultDBContext dbContext) : base(dbContext)
        {
        }

        public override Task<List<DataAccess.Models.User>> GetAll()
        {


            return _dbSet.AsNoTracking().Include(u => u.IdentityRoles).ToListAsync();
        }
    }
}