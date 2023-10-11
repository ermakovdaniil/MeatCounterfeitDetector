using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace MeatAPI.Features.User
{
    public class UserService : EntityAccessServiceBase<ResultDBContext, DataAccess.Models.User>
    {
        public UserService(ResultDBContext dbContext) : base(dbContext)
        {
        }

        public Task<DataAccess.Models.User> GetUserByLoginAndPassword(string login, string password)
        {
            return _dbSet.AsNoTracking().FirstAsync(u => u.Login == login && u.Password == password);
        }
    }
}
