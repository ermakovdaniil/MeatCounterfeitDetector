using DataAccess.Models;
using Mapster;
using MeatAPI.Features.CounterfeitPath.DTO;
using MeatAPI.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace MeatAPI.Features.CounterfeitPath
{
    public class CounterfeitPathService
    {
        private readonly IGenericRepository<DataAccess.Models.CounterfeitPath> _counterfeitPathRepository;


        public CounterfeitPathService(IGenericRepository<DataAccess.Models.CounterfeitPath> counterfeitPathRepository)
        {
            _counterfeitPathRepository = counterfeitPathRepository;
        }

        public async Task<IReadOnlyCollection<GetCounterfeitPathDTO>> GetAll()
        {
            var cp = await _counterfeitPathRepository.Get();
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
            var cp = await _counterfeitPathRepository.FindById(id);

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
            //cp.AppUserId = userId;
            await _counterfeitPathRepository.Create(cp);

            return cp.Id;
        }

        /// <summary>
        ///     Обновление объекта
        /// </summary>
        /// <param name="dto"> DTO с обновленной информацией об объекте</param>
        public async Task Update(UpdateCounterfeitPathDTO dto)
        {
            var cp = await _counterfeitPathRepository.FindById((Guid)dto.Id);
            dto.Adapt(cp);
            await _counterfeitPathRepository.Update(cp);
        }

        /// <summary>
        ///     Удаление объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        public async Task Delete(Guid id)
        {
            var cp = await _counterfeitPathRepository.FindById(id);
            await _counterfeitPathRepository.Remove(cp);
        }
    }
}
