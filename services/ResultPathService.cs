using DataAccess.Data;
using Mapster;
using MeatAPI.Features.ResultPath.DTO;
using Microsoft.EntityFrameworkCore;

namespace MeatAPI.Features.services
{
    public class ResultPathService
    {
        private readonly ResultDBContext _resultDBContext;

        public ResultPathService(ResultDBContext resultDBContext)
        {
            _resultDBContext = resultDBContext;
        }

        public async Task<IReadOnlyCollection<GetResultPathDTO>> GetAll()
        {
            var rp = await _resultDBContext.ResultPaths.AsNoTracking().ToListAsync();
            var dtos = rp.Adapt<List<GetResultPathDTO>>();
            return dtos;
        }

        /// <summary>
        ///     Получение объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        /// <returns>Информация о найденном объекте</returns>
        public async Task<GetResultPathDTO> Get(Guid id)
        {
            var rp = await _resultDBContext.ResultPaths.AsNoTracking().FirstAsync(rp => rp.Id == id);
            var rpDto = rp.Adapt<GetResultPathDTO>();
            return rpDto;
        }

        /// <summary>
        ///     Создание объекта
        /// </summary>
        /// <param name="dto">DTO с информацией о создаваемом объекте</param>
        /// <returns>ID созданного объекта</returns>
        public async Task<Guid> Create(CreateResultPathDTO dto)
        {
            var rp = dto.Adapt<DataAccess.Models.ResultPath>();
            await _resultDBContext.ResultPaths.AddAsync(rp);
            await _resultDBContext.SaveChangesAsync();
            return rp.Id;
        }

        /// <summary>
        ///     Обновление объекта
        /// </summary>
        /// <param name="dto"> DTO с обновленной информацией об объекте</param>
        public async Task Update(UpdateResultPathDTO dto)
        {
            var rp = await _resultDBContext.ResultPaths.FirstAsync(rp => rp.Id == (Guid)dto.Id);
            dto.Adapt(rp);
            await _resultDBContext.SaveChangesAsync();
        }

        /// <summary>
        ///     Удаление объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        public async Task Delete(Guid id)
        {
            var rp = await _resultDBContext.ResultPaths.FirstAsync(rp => rp.Id == id);
            _resultDBContext.Remove(rp);
            await _resultDBContext.SaveChangesAsync();
        }
    }
}
