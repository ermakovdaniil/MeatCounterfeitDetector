using DataAccess.Data;
using Mapster;
using MeatAPI.Features.OriginalPath.DTO;
using Microsoft.EntityFrameworkCore;

namespace MeatAPI.Features.services
{
    public class OriginalPathService
    {
        private readonly ResultDBContext _resultDBContext;

        public OriginalPathService(ResultDBContext resultDBContext)
        {
            _resultDBContext = resultDBContext;
        }

        public async Task<IReadOnlyCollection<GetOriginalPathDTO>> GetAll()
        {
            var op = await _resultDBContext.OriginalPaths.AsNoTracking().ToListAsync();
            var dtos = op.Adapt<List<GetOriginalPathDTO>>();
            return dtos;
        }

        /// <summary>
        ///     Получение объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        /// <returns>Информация о найденном объекте</returns>
        public async Task<GetOriginalPathDTO> Get(Guid id)
        {
            var op = await _resultDBContext.OriginalPaths.AsNoTracking().FirstAsync(op => op.Id == id);
            var opDto = op.Adapt<GetOriginalPathDTO>();
            return opDto;
        }

        /// <summary>
        ///     Создание объекта
        /// </summary>
        /// <param name="dto">DTO с информацией о создаваемом объекте</param>
        /// <returns>ID созданного объекта</returns>
        public async Task<Guid> Create(CreateOriginalPathDTO dto)
        {
            var op = dto.Adapt<DataAccess.Models.OriginalPath>();
            await _resultDBContext.OriginalPaths.AddAsync(op);
            await _resultDBContext.SaveChangesAsync();
            return op.Id;
        }

        /// <summary>
        ///     Обновление объекта
        /// </summary>
        /// <param name="dto"> DTO с обновленной информацией об объекте</param>
        public async Task Update(UpdateOriginalPathDTO dto)
        {
            var op = await _resultDBContext.OriginalPaths.FirstAsync(op => op.Id == (Guid)dto.Id);
            dto.Adapt(op);
            await _resultDBContext.SaveChangesAsync();
        }

        /// <summary>
        ///     Удаление объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        public async Task Delete(Guid id)
        {
            var op = await _resultDBContext.OriginalPaths.FirstAsync(op => op.Id == id);
            _resultDBContext.Remove(op);
            await _resultDBContext.SaveChangesAsync();
        }
    }
}
