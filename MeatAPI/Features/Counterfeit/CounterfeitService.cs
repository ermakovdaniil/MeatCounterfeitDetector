using DataAccess.Data;
using Mapster;
using MeatAPI.Features.Counterfeit.DTO;
using Microsoft.EntityFrameworkCore;

namespace MeatAPI.Features.Counterfeit
{
    public class CounterfeitService
    {
        private readonly CounterfeitKBContext _counterfeitKBContext;

        public CounterfeitService(CounterfeitKBContext counterfeitKBContext)
        {
            _counterfeitKBContext = counterfeitKBContext;
        }

        public async Task<IReadOnlyCollection<GetCounterfeitDTO>> GetAll()
        {
            var c = await _counterfeitKBContext.Counterfeits.AsNoTracking().ToListAsync();
            var dtos = c.Adapt<List<GetCounterfeitDTO>>();
            return dtos;
        }

        /// <summary>
        ///     Получение объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        /// <returns>Информация о найденном объекте</returns>
        public async Task<GetCounterfeitDTO> Get(Guid id)
        {
            var c = await _counterfeitKBContext.Counterfeits.AsNoTracking().FirstAsync(c => c.Id == id);
            var cDto = c.Adapt<GetCounterfeitDTO>();
            return cDto;
        }

        /// <summary>
        ///     Создание объекта
        /// </summary>
        /// <param name="dto">DTO с информацией о создаваемом объекте</param>
        /// <returns>ID созданного объекта</returns>
        public async Task<Guid> Create(CreateCounterfeitDTO dto)
        {
            var c = dto.Adapt<DataAccess.Models.Counterfeit>();
            await _counterfeitKBContext.Counterfeits.AddAsync(c);
            await _counterfeitKBContext.SaveChangesAsync();
            return c.Id;
        }

        /// <summary>
        ///     Обновление объекта
        /// </summary>
        /// <param name="dto"> DTO с обновленной информацией об объекте</param>
        public async Task Update(UpdateCounterfeitDTO dto)
        {
            var c = await _counterfeitKBContext.Counterfeits.FirstAsync(c => c.Id == (Guid)dto.Id);
            dto.Adapt(c);
            await _counterfeitKBContext.SaveChangesAsync();
        }

        /// <summary>
        ///     Удаление объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        public async Task Delete(Guid id)
        {
            var c = await _counterfeitKBContext.Counterfeits.FirstAsync(c => c.Id == id);
            _counterfeitKBContext.Remove(c);
            await _counterfeitKBContext.SaveChangesAsync();
        }
    }
}
