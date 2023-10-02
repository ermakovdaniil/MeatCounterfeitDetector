using Mapster;
using MeatAPI.Features.ResultPath.DTO;
using MeatAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.ResultPath
{
    public class ResultPathController : BaseAuthorizedController
    {
        private readonly IGenericRepository<DataAccess.Models.ResultPath> _resultPathRepository;
        private readonly ResultPathService _resultPathService;

        public ResultPathController(IGenericRepository<DataAccess.Models.ResultPath> resultPathRepository,
                               ResultPathService resultPathService)
        {
            _resultPathRepository = resultPathRepository;
            _resultPathService = resultPathService;
        }

        /// <summary>
        ///     Получение всех объектов
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<GetResultPathDTO>>> GetAll()
        {
            var resultPaths = await _resultPathRepository.Get();
            var resultPathsDTO = resultPaths.Adapt<List<GetResultPathDTO>>();

            return Ok(resultPathsDTO);
        }

        /// <summary>
        ///     Получение объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetResultPathDTO>> Get(Guid id)
        {
            var rp = await _resultPathService.Get(id);

            return Ok(rp);
        }

        /// <summary>
        ///     Создание объекта
        /// </summary>
        /// <param name="dto">DTO с информацией об объекте</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateResultPathDTO dto)
        {
            var id = await _resultPathService.Create(dto);

            return Ok(id);
        }

        /// <summary>
        ///     Обновление объекта
        /// </summary>
        /// <param name="dto">DTO с информацией об объекте</param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Update([FromBody] UpdateResultPathDTO dto)
        {
            await _resultPathService.Update(dto);

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
            await _resultPathService.Delete(id);

            return Ok();
        }
    }
}
