using DataAccess.Models;
using Mapster;
using MeatAPI.Features.Result.DTO;
using MeatAPI.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace MeatAPI.Features.Result
{
    public class ResultService
    {
        private readonly IGenericRepository<DataAccess.Models.Result> _resultRepository;


        public ResultService(IGenericRepository<DataAccess.Models.Result> resultRepository)
        {
            _resultRepository = resultRepository;
        }

        public async Task<IReadOnlyCollection<GetResultDTO>> GetAll()
        {
            var r = await _resultRepository.Get();
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
            var r = await _resultRepository.FindById(id);

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
            //r.AppResultId = resultId;
            await _resultRepository.Create(r);

            return r.Id;
        }


        /// <summary>
        ///     Удаление объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        public async Task Delete(Guid id)
        {
            var r = await _resultRepository.FindById(id);
            await _resultRepository.Remove(r);
        }
    }
}
