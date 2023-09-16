using DataAccess.Models;
using Mapster;
using MeatAPI.Features.OriginalPath.DTO;
using MeatAPI.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace MeatAPI.Features.OriginalPath
{
    public class OriginalPathService
    {
        private readonly IGenericRepository<DataAccess.Models.OriginalPath> _originalPathRepository;


        public OriginalPathService(IGenericRepository<DataAccess.Models.OriginalPath> originalPathRepository)
        {
            _originalPathRepository = originalPathRepository;
        }

        public async Task<IReadOnlyCollection<GetOriginalPathDTO>> GetAll()
        {
            var op = await _originalPathRepository.Get();
            var dtos = op.Adapt<List<GetOriginalPathDTO>>();

            return dtos;
        }

        /// <summary>
        ///     Получение объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        /// <returns>Информация о найденном объекте</returns>
        public async Task<GetOriginalPathDTO> Get(Guid id)
        {
            var op = await _originalPathRepository.FindById(id);

            var opDto = op.Adapt<GetOriginalPathDTO>();

            return opDto;
        }

        /// <summary>
        ///     Создание объекта
        /// </summary>
        /// <param name="dto">DTO с информацией о создаваемом объекте</param>
        /// <returns>ID созданного объекта</returns>
        public async Task<Guid> Create(CreateOriginalPathDTO dto)
        {
            var op = dto.Adapt<DataAccess.Models.OriginalPath>();
            //op.AppUserId = userId;
            await _originalPathRepository.Create(op);

            return op.Id;
        }

        /// <summary>
        ///     Обновление объекта
        /// </summary>
        /// <param name="dto"> DTO с обновленной информацией об объекте</param>
        public async Task Update(UpdateOriginalPathDTO dto)
        {
            var op = await _originalPathRepository.FindById((Guid)dto.Id);
            dto.Adapt(op);
            await _originalPathRepository.Update(op);
        }

        /// <summary>
        ///     Удаление объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        public async Task Delete(Guid id)
        {
            var op = await _originalPathRepository.FindById(id);
            await _originalPathRepository.Remove(op);
        }
    }
}
