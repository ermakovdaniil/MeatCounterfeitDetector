using ClientAPI.DTO.UserType;
using DataAccess.Data;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.UserType
{
    public class UserTypeController : BaseAuthorizedController
    {
        private readonly EntityAccessServiceBase<ResultDBContext, DataAccess.Models.UserType> _userTypeService;

        public UserTypeController(EntityAccessServiceBase<ResultDBContext, DataAccess.Models.UserType> userTypeService)
        {
            _userTypeService = userTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<GetUserTypeDTO>>> GetAll()
        {
            var userTypes = await _userTypeService.GetAll();
            var userTypesDTO = userTypes.Adapt<List<GetUserTypeDTO>>();
            return Ok(userTypesDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetUserTypeDTO>> Get(Guid id)
        {
            var ut = await _userTypeService.Get(id);
            var utDto = ut.Adapt<GetUserTypeDTO>();
            return Ok(utDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateUserTypeDTO dto)
        {
            var ut = dto.Adapt<DataAccess.Models.UserType>();
            var id = await _userTypeService.Create(ut);
            return Ok(id);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Update([FromBody] UpdateUserTypeDTO dto)
        {
            var ut = dto.Adapt<DataAccess.Models.UserType>();
            await _userTypeService.Update(ut);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _userTypeService.Delete(id);
            return Ok();
        }
    }
}
