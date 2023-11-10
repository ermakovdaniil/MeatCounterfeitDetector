using ClientAPI.DTO.ResultImage;
using DataAccess.Data;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.ResultImage
{
    public class ResultImageController : BaseAuthorizedController
    {
        private readonly EntityAccessServiceBase<ResultDBContext, DataAccess.Models.ResultImage> _resultImageService;

        public ResultImageController(EntityAccessServiceBase<ResultDBContext, DataAccess.Models.ResultImage> resultImageService)
        {
            _resultImageService = resultImageService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<GetResultImageDTO>>> GetAll()
        {
            var resultImages = await _resultImageService.GetAll();
            var resultImagesDTO = resultImages.Adapt<List<GetResultImageDTO>>();
            return Ok(resultImagesDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetResultImageDTO>> Get(Guid id)
        {
            var ri = await _resultImageService.Get(id);
            var riDto = ri.Adapt<GetResultImageDTO>();
            return Ok(riDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateResultImageDTO dto)
        {
            var ri = dto.Adapt<DataAccess.Models.ResultImage>();
            var id = await _resultImageService.Create(ri);
            return Ok(id);
        }

        //[HttpPut]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult> Update([FromBody] UpdateResultImageDTO dto)
        //{
        //    var rp = dto.Adapt<DataAccess.Models.ResultImage>();
        //    await _resultImageService.Update(rp);
        //    return Ok();
        //}

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _resultImageService.Delete(id);
            return Ok();
        }
    }
}
