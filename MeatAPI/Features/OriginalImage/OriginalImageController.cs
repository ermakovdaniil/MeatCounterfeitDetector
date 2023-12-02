using ClientAPI.DTO.CounterfeitImage;
using ClientAPI.DTO.OriginalImage;
using DataAccess.Data;
using Mapster;
using MeatAPI.Features.CounterfeitImage;
using Microsoft.AspNetCore.Mvc;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.OriginalImage
{
    public class OriginalImageController : BaseAuthorizedController
    {
        private readonly OriginalImageService _originalImageService;

        public OriginalImageController(OriginalImageService originalImageService)
        {
            _originalImageService = originalImageService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<GetOriginalImageDTO>>> GetAll()
        {
            var originalImages = await _originalImageService.GetAll();
            var originalImagesDTO = originalImages.Adapt<List<GetOriginalImageDTO>>();
            return Ok(originalImagesDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetOriginalImageDTO>> Get(Guid id)
        {
            var oi = await _originalImageService.Get(id);
            var oiDto = oi.Adapt<GetOriginalImageDTO>();
            return Ok(oiDto);
        }

        [HttpGet]
        [Route("GetAllByCounterfeitId/{counterfeitId}")]
        public async Task<ActionResult<IReadOnlyCollection<GetCounterfeitImageDTO>>> GetIdByName(string imagePath)
        {
            var oi = await _originalImageService.GetIdByName(imagePath);
            var oiDto = oi.Adapt<GetOriginalImageDTO>();
            return Ok(oiDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateOriginalImageDTO dto)
        {
            var oi = dto.Adapt<DataAccess.Models.OriginalImage>();
            var id = await _originalImageService.Create(oi);
            return Ok(id);
        }

        //[HttpPut]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult> Update([FromBody] UpdateOriginalImageDTO dto)
        //{
        //    var op = dto.Adapt<DataAccess.Models.OriginalImage>();
        //    await _originalImageService.Update(op);
        //    return Ok();
        //}

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _originalImageService.Delete(id);
            return Ok();
        }
    }
}
