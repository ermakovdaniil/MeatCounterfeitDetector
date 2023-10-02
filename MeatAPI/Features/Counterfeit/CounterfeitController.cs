using Mapster;
using MeatAPI.Features.Counterfeit.DTO;
using MeatAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.Counterfeit
{
    public class CounterfeitController : BaseAuthorizedController
    {
        private readonly IGenericRepository<DataAccess.Models.Counterfeit> _counterfeitRepository;
        private readonly CounterfeitService _counterfeitService;

        public CounterfeitController(IGenericRepository<DataAccess.Models.Counterfeit> counterfeitRepository,
                               CounterfeitService counterfeitService)
        {
            _counterfeitRepository = counterfeitRepository;
            _counterfeitService = counterfeitService;
        }

        /// <summary>
        ///     Получение всех объектов
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<GetCounterfeitDTO>>> GetAll()
        {
            var counterfeits = await _counterfeitRepository.Get();
            var counterfeitsDTO = counterfeits.Adapt<List<GetCounterfeitDTO>>();

            return Ok(counterfeitsDTO);
        }

        /// <summary>
        ///     Получение объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetCounterfeitDTO>> Get(Guid id)
        {
            var c = await _counterfeitService.Get(id);

            return Ok(c);
        }

        /// <summary>
        ///     Создание объекта
        /// </summary>
        /// <param name="dto">DTO с информацией об объекте</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateCounterfeitDTO dto)
        {
            var id = await _counterfeitService.Create(dto);

            return Ok(id);
        }

        /// <summary>
        ///     Обновление объекта
        /// </summary>
        /// <param name="dto">DTO с информацией об объекте</param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Update([FromBody] UpdateCounterfeitDTO dto)
        {
            await _counterfeitService.Update(dto);

            return Ok();
        }

        /// <summary>
        ///     Удаление объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _counterfeitService.Delete(id);

            return Ok();
        }
    }
}
