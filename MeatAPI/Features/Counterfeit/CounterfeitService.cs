using MeatAPI.Features.Counterfeit.DTO;
using MeatAPI.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace MeatAPI.Features.Counterfeit
{
    public class CounterfeitService
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IGenericRepository<DataAccess.Models.Counterfeit> _counterfeitRepository;


        public CounterfeitService(IGenericRepository<DataAccess.Models.Counterfeit> counterfeitRepository,
            IAuthorizationService authorizationService)
        {
            _counterfeitRepository = counterfeitRepository;
            _authorizationService = authorizationService;
        }

        public async Task<IReadOnlyCollection<GetCounterfeitDTO>> GetAll()
        {
            var l = await _counterfeitRepository.Get();
            var dtos = l.Adapt<List<GetCounterfeitDTO>>();

            return dtos;
        }

        /// <summary>
        ///     Получает слой по ID
        /// </summary>
        /// <param name="id">ID слоя</param>
        /// <returns>Информация о найденном слое</returns>
        public async Task<GetCounterfeitDTO> Get(Guid id)
        {
            var l = await _counterfeitRepository.FindById(id);

            var counterfeit = l.Adapt<GetCounterfeitDTO>();

            return counterfeit;
        }

        /// <summary>
        ///     Создает слой
        /// </summary>
        /// <param name="dto">DTO с информацией о создаваемом объекте</param>
        /// <param name="userId"> ID пользователя</param>
        /// <returns>ID созданного слоя</returns>
        public async Task<Guid> Create(CreateCounterfeitDTO dto)
        {
            var counterfeit = dto.Adapt<DataAccess.Counterfeit>();
            counterfeit.AppUserId = userId;
            counterfeit.ObjectsOnMap = new List<DataAccess.ObjectOnMap>();

            //todo асинхронно сделать, сейчас заглушка
            foreach (var objectId in dto.Objects)
            {
                counterfeit.ObjectsOnMap.Add(_objectOnMapRepository.FindById(objectId).Result);
            }

            await _counterfeitRepository.Create(counterfeit);

            return counterfeit.Id;
        }

        /// <summary>
        ///     Обнолвяет информацию о слое
        /// </summary>
        /// <param name="dto">DTO с информацией о слое</param>
        public async Task Update(UpdateCounterfeitDTO dto)
        {
            var counterfeit = await _counterfeitRepository.FindById((Guid)dto.Id);
            dto.Adapt(counterfeit);

            if (dto.Objects is not null)
            {
                counterfeit.ObjectsOnMap.Clear();

                foreach (var oId in dto.Objects)
                {
                    var o = await _objectOnMapRepository.FindById(oId);
                    counterfeit.ObjectsOnMap.Add(o);
                }
            }

            await _counterfeitRepository.Update(counterfeit);
        }

        /// <summary>
        ///     Удаляет слой по ID
        /// </summary>
        /// <param name="id">ID слоя</param>
        public async Task Delete(Guid id)
        {
            var counterfeitToDelete = await _counterfeitRepository.FindById(id);
            counterfeitToDelete.ObjectsOnMap.Clear();
            await _counterfeitRepository.Update(counterfeitToDelete);
            await _counterfeitRepository.Remove(counterfeitToDelete);
        }
    }
}
