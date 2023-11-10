using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace MeatAPI.Features.CounterfeitImage
{
    public class CounterfeitImageService : EntityAccessServiceBase<CounterfeitKBContext, DataAccess.Models.CounterfeitImage>
    {
        public CounterfeitImageService(CounterfeitKBContext dbContext) : base(dbContext)
        {
        }

        public override Task<List<DataAccess.Models.CounterfeitImage>> GetAll()
        {
            return _dbSet.AsNoTracking().Include(c => c.Counterfeit).ToListAsync();
        }

        public Task<List<DataAccess.Models.CounterfeitImage>> GetPathsByCounterfeitId(Guid conterfeitId)
        {
            return _dbSet.AsNoTracking().Where(c => c.CounterfeitId == conterfeitId).ToListAsync();
        }
    }
}