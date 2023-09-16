using DataAccess.Models;
using Mapster;
using MeatAPI.Features.User.DTO;
using MeatAPI.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace MeatAPI.Features.User
{
    public class UserService
    {
        private readonly IGenericRepository<DataAccess.Models.User> _userRepository;


        public UserService(IGenericRepository<DataAccess.Models.User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IReadOnlyCollection<GetUserDTO>> GetAll()
        {
            var u = await _userRepository.Get();
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
            var u = await _userRepository.FindById(id);

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
            //u.AppUserId = userId;
            await _userRepository.Create(u);

            return u.Id;
        }

        /// <summary>
        ///     Обновление объекта
        /// </summary>
        /// <param name="dto"> DTO с обновленной информацией об объекте</param>
        public async Task Update(UpdateUserDTO dto)
        {
            var u = await _userRepository.FindById((Guid)dto.Id);
            dto.Adapt(u);
            await _userRepository.Update(u);
        }

        /// <summary>
        ///     Удаление объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        public async Task Delete(Guid id)
        {
            var u = await _userRepository.FindById(id);
            await _userRepository.Remove(u);
        }
    }
}
