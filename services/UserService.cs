using DataAccess.Data;
using Mapster;
using MeatAPI.Features.User.DTO;
using Microsoft.EntityFrameworkCore;

namespace MeatAPI.Features.services
{
    public class UserService
    {
        private readonly ResultDBContext _resultDBContext;

        public UserService(ResultDBContext resultDBContext)
        {
            _resultDBContext = resultDBContext;
        }

        public async Task<IReadOnlyCollection<GetUserDTO>> GetAll()
        {
            var u = await _resultDBContext.Users.AsNoTracking().ToListAsync();
            var dtos = u.Adapt<List<GetUserDTO>>();
            return dtos;
        }

        /// <summary>
        ///     Получение объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        /// <returns>Информация о найденном объекте</returns>
        public async Task<GetUserDTO> Get(Guid id)
        {
            var u = await _resultDBContext.Users.AsNoTracking().FirstAsync(u => u.Id == id);
            var uDto = u.Adapt<GetUserDTO>();
            return uDto;
        }

        /// <summary>
        ///     Создание объекта
        /// </summary>
        /// <param name="dto">DTO с информацией о создаваемом объекте</param>
        /// <returns>ID созданного объекта</returns>
        public async Task<Guid> Create(CreateUserDTO dto)
        {
            var u = dto.Adapt<DataAccess.Models.User>();
            await _resultDBContext.Users.AddAsync(u);
            await _resultDBContext.SaveChangesAsync();
            return u.Id;
        }

        /// <summary>
        ///     Обновление объекта
        /// </summary>
        /// <param name="dto"> DTO с обновленной информацией об объекте</param>
        public async Task Update(UpdateUserDTO dto)
        {
            var u = await _resultDBContext.Users.FirstAsync(u => u.Id == (Guid)dto.Id);
            dto.Adapt(u);
            await _resultDBContext.SaveChangesAsync();
        }

        /// <summary>
        ///     Удаление объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        public async Task Delete(Guid id)
        {
            var u = await _resultDBContext.Users.FirstAsync(u => u.Id == id);
            _resultDBContext.Remove(u);
            await _resultDBContext.SaveChangesAsync();
        }
    }
}
