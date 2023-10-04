using DataAccess.Data;
using Mapster;
using MeatAPI.Features.UserType.DTO;
using Microsoft.EntityFrameworkCore;

namespace MeatAPI.Features.UserType
{
    public class UserTypeService
    {
        private readonly ResultDBContext _resultDBContext;

        public UserTypeService(ResultDBContext resultDBContext)
        {
            _resultDBContext = resultDBContext;
        }

        public async Task<IReadOnlyCollection<GetUserTypeDTO>> GetAll()
        {
            var ut = await _resultDBContext.UserTypes.AsNoTracking().ToListAsync();
            var dtos = ut.Adapt<List<GetUserTypeDTO>>();
            return dtos;
        }

        /// <summary>
        ///     Получение объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        /// <returns>Информация о найденном объекте</returns>
        public async Task<GetUserTypeDTO> Get(Guid id)
        {
            var ut = await _resultDBContext.UserTypes.AsNoTracking().FirstAsync(ut => ut.Id == id);
            var utDto = ut.Adapt<GetUserTypeDTO>();
            return utDto;
        }

        /// <summary>
        ///     Создание объекта
        /// </summary>
        /// <param name="dto">DTO с информацией о создаваемом объекте</param>
        /// <returns>ID созданного объекта</returns>
        public async Task<Guid> Create(CreateUserTypeDTO dto)
        {
            var ut = dto.Adapt<DataAccess.Models.UserType>();
            await _resultDBContext.UserTypes.AddAsync(ut);
            await _resultDBContext.SaveChangesAsync();
            return ut.Id;
        }

        /// <summary>
        ///     Обновление объекта
        /// </summary>
        /// <param name="dto"> DTO с обновленной информацией об объекте</param>
        public async Task Update(UpdateUserTypeDTO dto)
        {
            var ut = await _resultDBContext.UserTypes.FirstAsync(ut => ut.Id == (Guid)dto.Id);
            dto.Adapt(ut);
            await _resultDBContext.SaveChangesAsync();
        }

        /// <summary>
        ///     Удаление объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        public async Task Delete(Guid id)
        {
            var ut = await _resultDBContext.UserTypes.FirstAsync(ut => ut.Id == id);
            _resultDBContext.Remove(ut);
            await _resultDBContext.SaveChangesAsync();
        }
    }
}
