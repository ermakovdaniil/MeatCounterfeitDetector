using ClientAPI.DTO.Counterfeit;
using DataAccess.Data;
using DataAccess.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.Counterfeit
{
    public class CounterfeitController : BaseAuthorizedController
    {
        private readonly EntityAccessServiceBase<CounterfeitKBContext, DataAccess.Models.Counterfeit> _counterfeitService;

        public CounterfeitController(EntityAccessServiceBase<CounterfeitKBContext, DataAccess.Models.Counterfeit> counterfeitService)
        {
            _counterfeitService = counterfeitService;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<IReadOnlyCollection<GetCounterfeitDTO>>> GetAll()
        {
            var counterfeits = await _counterfeitService.GetAll();
            var counterfeitsDTO = counterfeits.Adapt<List<GetCounterfeitDTO>>();
            return Ok(counterfeitsDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetCounterfeitDTO>> Get(Guid id)
        {
            var c = await _counterfeitService.Get(id);
            var cDto = c.Adapt<GetCounterfeitDTO>();
            return Ok(cDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateCounterfeitDTO dto)
        {
            var c = dto.Adapt<DataAccess.Models.Counterfeit>();
            var id = await _counterfeitService.Create(c);
            return Ok(id);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Update([FromBody] UpdateCounterfeitDTO dto)
        {
            var c = dto.Adapt<DataAccess.Models.Counterfeit>();
            await _counterfeitService.Update(c);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _counterfeitService.Delete(id);
            return Ok();
        }
    }
}
