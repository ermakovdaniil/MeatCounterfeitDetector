using ClientAPI.DTO.CounterfeitImage;
using DataAccess.Data;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.CounterfeitImage
{
    public class CounterfeitImageController : BaseAuthorizedController
    {
        private readonly CounterfeitImageService _counterfeitImageService;

        public CounterfeitImageController(CounterfeitImageService counterfeitImageService)
        {
            _counterfeitImageService = counterfeitImageService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<GetCounterfeitImageDTO>>> GetAll()
        {
            var counterfeitImages = await _counterfeitImageService.GetAll();
            //var getCounterfeitImageDTOs = new List<GetCounterfeitImageDTO>();
            //foreach (var cp in counterfeitImages)
            //{
            //    var dto = new GetCounterfeitImageDTO();
            //    dto.Id = cp.Id;
            //    dto.CounterfeitId = cp.CounterfeitId;
            //    dto.EncodedImage = cp.EncodedImage;
            //    dto.CounterfeitName = cp.Counterfeit.Name;
            //    getCounterfeitImageDTOs.Add(dto);
            //}

            var counterfeitImagesDTO = counterfeitImages.Adapt<List<GetCounterfeitImageDTO>>();
            return Ok(counterfeitImagesDTO);
        }


        [HttpGet]
        [Route("GetAllByCounterfeitId/{counterfeitId}")]
        public async Task<ActionResult<IReadOnlyCollection<GetCounterfeitImageDTO>>> GetAllByCounterfeitId(Guid counterfeitId)
        {
            var counterfeitImages = await _counterfeitImageService.GetPathsByCounterfeitId(counterfeitId);
            var counterfeitImagesDTO = counterfeitImages.Adapt<List<GetCounterfeitImageDTO>>();
            return Ok(counterfeitImagesDTO);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetCounterfeitImageDTO>> Get(Guid id)
        {
            var cp = await _counterfeitImageService.Get(id);
            var cpDto = cp.Adapt<GetCounterfeitImageDTO>();
            return Ok(cpDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateCounterfeitImageDTO dto)
        {
            var cp = dto.Adapt<DataAccess.Models.CounterfeitImage>();
            var id = await _counterfeitImageService.Create(cp);
            return Ok(id);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Update([FromBody] UpdateCounterfeitImageDTO dto)
        {
            var cp = dto.Adapt<DataAccess.Models.CounterfeitImage>();
            await _counterfeitImageService.Update(cp);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _counterfeitImageService.Delete(id);
            return Ok();
        }
    }
}
