using DataAccess.Models;
using Mapster;
using MeatAPI.Features.ResultPath.DTO;
using MeatAPI.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace MeatAPI.Features.ResultPath
{
    public class ResultPathService
    {
        private readonly IGenericRepository<DataAccess.Models.ResultPath> _resultPathRepository;


        public ResultPathService(IGenericRepository<DataAccess.Models.ResultPath> resultPathRepository)
        {
            _resultPathRepository = resultPathRepository;
        }

        public async Task<IReadOnlyCollection<GetResultPathDTO>> GetAll()
        {
            var rp = await _resultPathRepository.Get();
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
            var rp = await _resultPathRepository.FindById(id);

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
            //rp.AppUserId = userId;
            await _resultPathRepository.Create(rp);

            return rp.Id;
        }

        /// <summary>
        ///     Обновление объекта
        /// </summary>
        /// <param name="dto"> DTO с обновленной информацией об объекте</param>
        public async Task Update(UpdateResultPathDTO dto)
        {
            var rp = await _resultPathRepository.FindById((Guid)dto.Id);
            dto.Adapt(rp);
            await _resultPathRepository.Update(rp);
        }

        /// <summary>
        ///     Удаление объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        public async Task Delete(Guid id)
        {
            var rp = await _resultPathRepository.FindById(id);
            await _resultPathRepository.Remove(rp);
        }
    }
}
