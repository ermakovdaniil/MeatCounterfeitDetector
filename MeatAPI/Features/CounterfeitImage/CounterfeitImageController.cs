using ClientAPI.DTO.CounterfeitImage;
using Mapster;
using Microsoft.AspNetCore.Mvc;
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
            var getCounterfeitImageDTOs = new List<GetCounterfeitImageDTO>();
            foreach (var ci in counterfeitImages)
            {
                var dto = new GetCounterfeitImageDTO();
                dto.Id = ci.Id;
                dto.CounterfeitId = ci.CounterfeitId;
                dto.ImagePath = ci.ImagePath;
                dto.CounterfeitName = ci.Counterfeit.Name;
                getCounterfeitImageDTOs.Add(dto);
            }

            //var counterfeitImagesDTO = counterfeitImages.Adapt<List<GetCounterfeitImageDTO>>();
            return Ok(getCounterfeitImageDTOs);
        }


        [HttpGet]
        [Route("GetAllByCounterfeitId/{counterfeitId}")]
        public async Task<ActionResult<IReadOnlyCollection<GetCounterfeitImageDTO>>> GetAllByCounterfeitId(Guid counterfeitId)
        {
            var counterfeitImages = await _counterfeitImageService.GetPathsByCounterfeitId(counterfeitId);
            var getCounterfeitImageDTOs = new List<GetCounterfeitImageDTO>();
            foreach (var ci in counterfeitImages)
            {
                var dto = new GetCounterfeitImageDTO();
                dto.Id = ci.Id;
                dto.CounterfeitId = ci.CounterfeitId;
                dto.ImagePath = ci.ImagePath;
                dto.CounterfeitName = ci.Counterfeit.Name;
                getCounterfeitImageDTOs.Add(dto);
            }

            //var counterfeitImagesDTO = counterfeitImages.Adapt<List<GetCounterfeitImageDTO>>();
            return Ok(getCounterfeitImageDTOs);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetCounterfeitImageDTO>> Get(Guid id)
        {
            var ci = await _counterfeitImageService.Get(id);
            var ciDto = ci.Adapt<GetCounterfeitImageDTO>();
            return Ok(ciDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateCounterfeitImageDTO dto)
        {
            var ci = dto.Adapt<DataAccess.Models.CounterfeitImage>();
            var id = await _counterfeitImageService.Create(ci);
            return Ok(id);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Update([FromBody] UpdateCounterfeitImageDTO dto)
        {
            var ci = dto.Adapt<DataAccess.Models.CounterfeitImage>();
            await _counterfeitImageService.Update(ci);
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
