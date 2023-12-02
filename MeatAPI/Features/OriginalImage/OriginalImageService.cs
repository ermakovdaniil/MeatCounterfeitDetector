using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace MeatAPI.Features.OriginalImage
{
    public class OriginalImageService : EntityAccessServiceBase<ResultDBContext, DataAccess.Models.OriginalImage>
    {
        public OriginalImageService(ResultDBContext dbContext) : base(dbContext)
        {
        }

        public Task<DataAccess.Models.OriginalImage> GetIdByName(string imagePath)
        {
            return _dbSet.AsNoTracking().FirstAsync(oi => oi.ImagePath == imagePath);
        }
    }
}