using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace MeatAPI.Features.OriginalImage
{
    public class OriginalImageService : EntityAccessServiceBase<ResultDBContext, DataAccess.Models.OriginalImage>
    {
        public OriginalImageService(ResultDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Guid?> GetIdByName(string imagePath)
        {
            var entity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(oi => oi.ImagePath == imagePath);

            if (entity is not null)
            {
                return entity.Id;
            }
            else
            {
                return null;
            }
        }
    }
}