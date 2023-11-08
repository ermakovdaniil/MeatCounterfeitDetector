using ClientAPI.DTO.ResultPath;
using DataAccess.Data;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.ResultPath
{
    public class ResultPathController : BaseAuthorizedController
    {
        private readonly EntityAccessServiceBase<ResultDBContext, DataAccess.Models.ResultPath> _resultPathService;

        public ResultPathController(EntityAccessServiceBase<ResultDBContext, DataAccess.Models.ResultPath> resultPathService)
        {
            _resultPathService = resultPathService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<GetResultPathDTO>>> GetAll()
        {
            var resultPaths = await _resultPathService.GetAll();
            var resultPathsDTO = resultPaths.Adapt<List<GetResultPathDTO>>();
            return Ok(resultPathsDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetResultPathDTO>> Get(Guid id)
        {
            var rp = await _resultPathService.Get(id);
            var rpDto = rp.Adapt<GetResultPathDTO>();
            return Ok(rpDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateResultPathDTO dto)
        {
            var rp = dto.Adapt<DataAccess.Models.ResultPath>();
            var id = await _resultPathService.Create(rp);
            return Ok(id);
        }

        //[HttpPut]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult> Update([FromBody] UpdateResultPathDTO dto)
        //{
        //    var rp = dto.Adapt<DataAccess.Models.ResultPath>();
        //    await _resultPathService.Update(rp);
        //    return Ok();
        //}

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _resultPathService.Delete(id);
            return Ok();
        }
    }
}
