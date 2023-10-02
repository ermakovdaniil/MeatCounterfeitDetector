using Mapster;
using MeatAPI.Features.CounterfeitPath;
using MeatAPI.Features.Result.DTO;
using MeatAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.Result
{
    public class ResultController : BaseAuthorizedController
    {
        private readonly IGenericRepository<DataAccess.Models.Result> _resultRepository;
        private readonly ResultService _resultService;

        public ResultController(IGenericRepository<DataAccess.Models.Result> resultRepository,
                               ResultService resultService)
        {
            _resultRepository = resultRepository;
            _resultService = resultService;
        }

        /// <summary>
        ///     Получение всех объектов
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<GetResultDTO>>> GetAll()
        {
            var results = await _resultRepository.Get();
            var resultsDTO = results.Adapt<List<GetResultDTO>>();

            return Ok(resultsDTO);
        }

        /// <summary>
        ///     Получение объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetResultDTO>> Get(Guid id)
        {
            var r = await _resultService.Get(id);

            return Ok(r);
        }

        /// <summary>
        ///     Создание объекта
        /// </summary>
        /// <param name="dto">DTO с информацией об объекте</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateResultDTO dto)
        {
            var id = await _resultService.Create(dto);

            return Ok(id);
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
            await _resultService.Delete(id);

            return Ok();
        }
    }
}
