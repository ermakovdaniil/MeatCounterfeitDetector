using DataAccess.Data;
using Mapster;
using MeatAPI.Features.Counterfeit.DTO;
using Microsoft.EntityFrameworkCore;

namespace MeatAPI.Features.services
{
    public class CounterfeitService
    {
        private readonly CounterfeitKBContext _counterfeitKBContext;

        public CounterfeitService(CounterfeitKBContext counterfeitKBContext)
        {
            _counterfeitKBContext = counterfeitKBContext;
        }

        public Task<List<DataAccess.Models.Counterfeit>> GetAll()
        {
            return _counterfeitKBContext.Counterfeits.AsNoTracking().ToListAsync();
        }

        public Task<DataAccess.Models.Counterfeit> Get(Guid id)
        {
            return _counterfeitKBContext.Counterfeits.AsNoTracking().FirstAsync(c => c.Id == id);
        }

        public async Task<Guid> Create(DataAccess.Models.Counterfeit c)
        {
            await _counterfeitKBContext.Counterfeits.AddAsync(c);
            await _counterfeitKBContext.SaveChangesAsync();
            return c.Id;
        }

        //public async Task Update(UpdateCounterfeitDTO dto)
        //{
        //    var c = await _counterfeitKBContext.Counterfeits.FirstAsync(c => c.Id == (Guid)dto.Id);
        //    dto.Adapt(c);
        //    await _counterfeitKBContext.SaveChangesAsync();
        //}

        //public async Task Delete(Guid id)
        //{
        //    var c = await _counterfeitKBContext.Counterfeits.FirstAsync(c => c.Id == id);
        //    _counterfeitKBContext.Remove(c);
        //    await _counterfeitKBContext.SaveChangesAsync();
        //}
    }
}
