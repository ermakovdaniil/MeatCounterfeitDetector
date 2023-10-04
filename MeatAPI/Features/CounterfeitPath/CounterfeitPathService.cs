using DataAccess.Data;
using Mapster;
using MeatAPI.Features.CounterfeitPath.DTO;
using Microsoft.EntityFrameworkCore;

namespace MeatAPI.Features.CounterfeitPath
{
    public class CounterfeitPathService
    {
        private readonly CounterfeitKBContext _counterfeitKBContext;

        public CounterfeitPathService(CounterfeitKBContext counterfeitKBContext)
        {
            _counterfeitKBContext = counterfeitKBContext;
        }

        public async Task<IReadOnlyCollection<GetCounterfeitPathDTO>> GetAll()
        {
            var cp = await _counterfeitKBContext.CounterfeitPaths.AsNoTracking().ToListAsync();
            var dtos = cp.Adapt<List<GetCounterfeitPathDTO>>();
            return dtos;
        }

        /// <summary>
        ///     Получение объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        /// <returns>Информация о найденном объекте</returns>
        public async Task<GetCounterfeitPathDTO> Get(Guid id)
        {
            var cp = await _counterfeitKBContext.CounterfeitPaths.AsNoTracking().FirstAsync(cp => cp.Id == id);
            var cpDto = cp.Adapt<GetCounterfeitPathDTO>();
            return cpDto;
        }

        /// <summary>
        ///     Создание объекта
        /// </summary>
        /// <param name="dto">DTO с информацией о создаваемом объекте</param>
        /// <returns>ID созданного объекта</returns>
        public async Task<Guid> Create(CreateCounterfeitPathDTO dto)
        {
            var cp = dto.Adapt<DataAccess.Models.CounterfeitPath>();
            await _counterfeitKBContext.CounterfeitPaths.AddAsync(cp);
            await _counterfeitKBContext.SaveChangesAsync();
            return cp.Id;
        }

        /// <summary>
        ///     Обновление объекта
        /// </summary>
        /// <param name="dto"> DTO с обновленной информацией об объекте</param>
        public async Task Update(UpdateCounterfeitPathDTO dto)
        {
            var cp = await _counterfeitKBContext.CounterfeitPaths.FirstAsync(cp => cp.Id == (Guid)dto.Id);
            dto.Adapt(cp);
            await _counterfeitKBContext.SaveChangesAsync();
        }

        /// <summary>
        ///     Удаление объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        public async Task Delete(Guid id)
        {
            var cp = await _counterfeitKBContext.CounterfeitPaths.FirstAsync(cp => cp.Id == id);
            _counterfeitKBContext.Remove(cp);
            await _counterfeitKBContext.SaveChangesAsync();
        }
    }
}
