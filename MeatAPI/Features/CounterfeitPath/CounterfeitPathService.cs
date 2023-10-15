﻿using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace MeatAPI.Features.CounterfeitPath
{
    public class CounterfeitPathService : EntityAccessServiceBase<CounterfeitKBContext, DataAccess.Models.CounterfeitPath>
    {
        public CounterfeitPathService(CounterfeitKBContext dbContext) : base(dbContext)
        {
        }

        public Task<List<DataAccess.Models.CounterfeitPath>> GetPathsByCounterfeitId(Guid conterfeitId)
        {
            return _dbSet.AsNoTracking().Where(c => c.CounterfeitId == conterfeitId).ToListAsync();
        }
    }
}