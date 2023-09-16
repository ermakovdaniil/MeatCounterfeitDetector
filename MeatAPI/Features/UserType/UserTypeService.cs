using DataAccess.Models;
using Mapster;
using MeatAPI.Features.UserType.DTO;
using MeatAPI.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace MeatAPI.Features.UserType
{
    public class UserTypeService
    {
        private readonly IGenericRepository<DataAccess.Models.UserType> _userTypeRepository;


        public UserTypeService(IGenericRepository<DataAccess.Models.UserType> userTypeRepository)
        {
            _userTypeRepository = userTypeRepository;
        }

        public async Task<IReadOnlyCollection<GetUserTypeDTO>> GetAll()
        {
            var ut = await _userTypeRepository.Get();
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
            var ut = await _userTypeRepository.FindById(id);

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
            //ut.AppUserId = userId;
            await _userTypeRepository.Create(ut);

            return ut.Id;
        }

        /// <summary>
        ///     Обновление объекта
        /// </summary>
        /// <param name="dto"> DTO с обновленной информацией об объекте</param>
        public async Task Update(UpdateUserTypeDTO dto)
        {
            var ut = await _userTypeRepository.FindById((Guid)dto.Id);
            dto.Adapt(ut);
            await _userTypeRepository.Update(ut);
        }

        /// <summary>
        ///     Удаление объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        public async Task Delete(Guid id)
        {
            var ut = await _userTypeRepository.FindById(id);
            await _userTypeRepository.Remove(ut);
        }
    }
}
