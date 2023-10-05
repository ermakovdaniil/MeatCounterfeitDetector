using ClientAPI.DTO.OriginalPath;
using DataAccess.Data;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.OriginalPath
{
    public class OriginalPathController : BaseAuthorizedController
    {
        private readonly EntityAccessServiceBase<ResultDBContext, DataAccess.Models.OriginalPath> _originalPathService;

        public OriginalPathController(EntityAccessServiceBase<ResultDBContext, DataAccess.Models.OriginalPath> originalPathService)
        {
            _originalPathService = originalPathService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<GetOriginalPathDTO>>> GetAll()
        {
            var originalPaths = await _originalPathService.GetAll();
            var originalPathsDTO = originalPaths.Adapt<List<GetOriginalPathDTO>>();
            return Ok(originalPathsDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetOriginalPathDTO>> Get(Guid id)
        {
            var op = await _originalPathService.Get(id);
            var opDto = op.Adapt<GetOriginalPathDTO>();
            return Ok(opDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateOriginalPathDTO dto)
        {
            var op = dto.Adapt<DataAccess.Models.OriginalPath>();
            var id = await _originalPathService.Create(op);
            return Ok(id);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Update([FromBody] UpdateOriginalPathDTO dto)
        {
            var op = dto.Adapt<DataAccess.Models.OriginalPath>();
            await _originalPathService.Update(op);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _originalPathService.Delete(id);
            return Ok();
        }
    }
}
