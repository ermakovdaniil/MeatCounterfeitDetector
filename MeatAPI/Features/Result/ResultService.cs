using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace MeatAPI.Features.Result
{
    public class ResultService : EntityAccessServiceBase<ResultDBContext, DataAccess.Models.Result>
    {
        public ResultService(ResultDBContext dbContext) : base(dbContext)
        {
        }

        public override Task<List<DataAccess.Models.Result>> GetAll()
        {
            return _dbSet.AsNoTracking().Include(oi => oi.OriginalImage).Include(ri => ri.ResultImage).Include(u => u.User).ToListAsync();
        }
    }
}