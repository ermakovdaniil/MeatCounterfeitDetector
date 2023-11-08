using ClientAPI.DTO.CounterfeitPath;
using DataAccess.Data;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.CounterfeitPath
{
    public class CounterfeitPathController : BaseAuthorizedController
    {
        private readonly CounterfeitPathService _counterfeitPathService;

        public CounterfeitPathController(CounterfeitPathService counterfeitPathService)
        {
            _counterfeitPathService = counterfeitPathService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<GetCounterfeitPathDTO>>> GetAll()
        {
            var counterfeitPaths = await _counterfeitPathService.GetAll();
            //var getCounterfeitPathDTOs = new List<GetCounterfeitPathDTO>();
            //foreach (var cp in counterfeitPaths)
            //{
            //    var dto = new GetCounterfeitPathDTO();
            //    dto.Id = cp.Id;
            //    dto.CounterfeitId = cp.CounterfeitId;
            //    dto.EncodedImage = cp.EncodedImage;
            //    dto.CounterfeitName = cp.Counterfeit.Name;
            //    getCounterfeitPathDTOs.Add(dto);
            //}

            var counterfeitPathsDTO = counterfeitPaths.Adapt<List<GetCounterfeitPathDTO>>();
            return Ok(counterfeitPathsDTO);
        }


        [HttpGet]
        [Route("GetAllByCounterfeitId/{counterfeitId}")]
        public async Task<ActionResult<IReadOnlyCollection<GetCounterfeitPathDTO>>> GetAllByCounterfeitId(Guid counterfeitId)
        {
            var counterfeitPaths = await _counterfeitPathService.GetPathsByCounterfeitId(counterfeitId);
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
