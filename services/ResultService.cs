using DataAccess.Data;
using Mapster;
using MeatAPI.Features.Result.DTO;
using Microsoft.EntityFrameworkCore;

namespace MeatAPI.Features.services
{
    public class ResultService
    {
        private readonly ResultDBContext _resultDBContext;

        public ResultService(ResultDBContext resultDBContext)
        {
            _resultDBContext = resultDBContext;
        }

        public async Task<IReadOnlyCollection<GetResultDTO>> GetAll()
        {
            var r = await _resultDBContext.Results.AsNoTracking().ToListAsync();
            var dtos = r.Adapt<List<GetResultDTO>>();
            return dtos;
        }

        /// <summary>
        ///     Получение объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        /// <returns>Информация о найденном объекте</returns>
        public async Task<GetResultDTO> Get(Guid id)
        {
            var r = await _resultDBContext.Results.AsNoTracking().FirstAsync(r => r.Id == id);
            var rDto = r.Adapt<GetResultDTO>();
            return rDto;
        }

        /// <summary>
        ///     Создание объекта
        /// </summary>
        /// <param name="dto">DTO с информацией о создаваемом объекте</param>
        /// <returns>ID созданного объекта</returns>
        public async Task<Guid> Create(CreateResultDTO dto)
        {
            var r = dto.Adapt<DataAccess.Models.Result>();
            await _resultDBContext.Results.AddAsync(r);
            await _resultDBContext.SaveChangesAsync();
            return r.Id;
        }

        /// <summary>
        ///     Удаление объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        public async Task Delete(Guid id)
        {
            var r = await _resultDBContext.Results.FirstAsync(r => r.Id == id);
            _resultDBContext.Remove(r);
            await _resultDBContext.SaveChangesAsync();
        }
    }
}
