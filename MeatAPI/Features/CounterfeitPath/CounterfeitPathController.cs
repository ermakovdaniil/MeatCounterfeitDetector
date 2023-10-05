using ClientAPI.DTO.CounterfeitPath;
using DataAccess.Data;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.CounterfeitPath
{
    public class CounterfeitPathController : BaseAuthorizedController
    {
        private readonly EntityAccessServiceBase<CounterfeitKBContext, DataAccess.Models.CounterfeitPath> _counterfeitPathService;

        public CounterfeitPathController(EntityAccessServiceBase<CounterfeitKBContext, DataAccess.Models.CounterfeitPath> counterfeitPathService)
        {
            _counterfeitPathService = counterfeitPathService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<GetCounterfeitPathDTO>>> GetAll()
        {
            var counterfeitPaths = await _counterfeitPathService.GetAll();
            var counterfeitPathsDTO = counterfeitPaths.Adapt<List<GetCounterfeitPathDTO>>();
            return Ok(counterfeitPathsDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetCounterfeitPathDTO>> Get(Guid id)
        {
            var cp = await _counterfeitPathService.Get(id);
            var cpDto = cp.Adapt<GetCounterfeitPathDTO>();
            return Ok(cpDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateCounterfeitPathDTO dto)
        {
            var cp = dto.Adapt<DataAccess.Models.CounterfeitPath>();
            var id = await _counterfeitPathService.Create(cp);
            return Ok(id);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Update([FromBody] UpdateCounterfeitPathDTO dto)
        {
            var cp = dto.Adapt<DataAccess.Models.CounterfeitPath>();
            await _counterfeitPathService.Update(cp);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _counterfeitPathService.Delete(id);
            return Ok();
        }
    }
}
