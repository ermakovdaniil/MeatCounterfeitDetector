using Mapster;
using MeatAPI.Features.OriginalPath.DTO;
using MeatAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.OriginalPath
{
    public class OriginalPathController : BaseAuthorizedController
    {
        private readonly IGenericRepository<DataAccess.Models.OriginalPath> _counterfeitPathRepository;
        private readonly OriginalPathService _counterfeitPathService;

        public OriginalPathController(IGenericRepository<DataAccess.Models.OriginalPath> counterfeitRepository,
                               OriginalPathService counterfeitService)
        {
            _counterfeitPathRepository = counterfeitRepository;
            _counterfeitPathService = counterfeitService;
        }

        /// <summary>
        ///     Получение всех объектов
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<GetOriginalPathDTO>>> GetAll()
        {
            var counterfeits = await _counterfeitPathRepository.Get();
            var counterfeitsDTO = counterfeits.Adapt<List<GetOriginalPathDTO>>();

            return Ok(counterfeitsDTO);
        }

        /// <summary>
        ///     Получение объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetOriginalPathDTO>> Get(Guid id)
        {
            var cp = await _counterfeitPathService.Get(id);

            return Ok(cp);
        }

        /// <summary>
        ///     Создание объекта
        /// </summary>
        /// <param name="dto">DTO с информацией об объекте</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateOriginalPathDTO dto)
        {
            var id = await _counterfeitPathRepository.Create(dto);

            return Ok(dto);
        }

        /// <summary>
        ///     Обновление объекта
        /// </summary>
        /// <param name="dto">DTO с информацией об объекте</param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Update([FromBody] UpdateOriginalPathDTO dto)
        {
            var obj = await _counterfeitPathRepository.FindById((Guid)dto.Id);

            await _counterfeitPathRepository.Update(dto);

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
            await _counterfeitPathService.Delete(id);

            return Ok();
        }
    }
}
