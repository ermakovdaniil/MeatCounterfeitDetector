using DataAccess.Models;
using Mapster;
using MeatAPI.Features.Counterfeit.DTO;
using MeatAPI.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace MeatAPI.Features.Counterfeit
{
    public class CounterfeitService
    {
        private readonly IGenericRepository<DataAccess.Models.Counterfeit> _counterfeitRepository;


        public CounterfeitService(IGenericRepository<DataAccess.Models.Counterfeit> counterfeitRepository)
        {
            _counterfeitRepository = counterfeitRepository;
        }

        public async Task<IReadOnlyCollection<GetCounterfeitDTO>> GetAll()
        {
            var c = await _counterfeitRepository.Get();
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
            var c = await _counterfeitRepository.FindById(id);

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
            //c.AppUserId = userId;
            await _counterfeitRepository.Create(c);

            return c.Id;
        }

        /// <summary>
        ///     Обновление объекта
        /// </summary>
        /// <param name="dto"> DTO с обновленной информацией об объекте</param>
        public async Task Update(UpdateCounterfeitDTO dto)
        {
            var c = await _counterfeitRepository.FindById((Guid)dto.Id);
            dto.Adapt(c);
            await _counterfeitRepository.Update(c);
        }

        /// <summary>
        ///     Удаление объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        public async Task Delete(Guid id)
        {
            var c = await _counterfeitRepository.FindById(id);
            await _counterfeitRepository.Remove(c);
        }
    }
}
