using Mapster;
using MeatAPI.Features.CounterfeitPath.DTO;
using MeatAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.CounterfeitPath
{
    public class CounterfeitPathController : BaseAuthorizedController
    {
        private readonly IGenericRepository<DataAccess.Models.CounterfeitPath> _counterfeitPathRepository;
        private readonly CounterfeitPathService _counterfeitPathService;

        public CounterfeitPathController(IGenericRepository<DataAccess.Models.CounterfeitPath> counterfeitPathRepository,
                               CounterfeitPathService counterfeitPathService)
        {
            _counterfeitPathRepository = counterfeitPathRepository;
            _counterfeitPathService = counterfeitPathService;
        }

        /// <summary>
        ///     Получение всех объектов
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<GetCounterfeitPathDTO>>> GetAll()
        {
            var counterfeitPaths = await _counterfeitPathRepository.Get();
            var counterfeitPathsDTO = counterfeitPaths.Adapt<List<GetCounterfeitPathDTO>>();

            return Ok(counterfeitPathsDTO);
        }

        /// <summary>
        ///     Получение объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetCounterfeitPathDTO>> Get(Guid id)
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
        public async Task<ActionResult<Guid>> Create([FromBody] CreateCounterfeitPathDTO dto)
        {
            var id = await _counterfeitPathService.Create(dto);

            return Ok(id);
        }

        /// <summary>
        ///     Обновление объекта
        /// </summary>
        /// <param name="dto">DTO с информацией об объекте</param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Update([FromBody] UpdateCounterfeitPathDTO dto)
        {
            await _counterfeitPathService.Update(dto);

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
