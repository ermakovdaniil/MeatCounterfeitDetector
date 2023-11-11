using ClientAPI.DTO.User;
using DataAccess.Data;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.User
{
    public class UserController : BaseAuthorizedController
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<GetUserDTO>>> GetAll()
        {
            var usersDTO = await _userService.GetAll();
            return Ok(usersDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetUserDTO>> Get(Guid id)
        {
            var u = await _userService.Get(id);
            var uDto = u.Adapt<GetUserDTO>();
            return Ok(uDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateUserDTO dto)
        {
            var u = dto.Adapt<DataAccess.Models.User>();
            var id = await _userService.Create(u);
            return Ok(id);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Update([FromBody] UpdateUserDTO dto)
        {
            var u = dto.Adapt<DataAccess.Models.User>();
            await _userService.Update(u);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _userService.Delete(id);
            return Ok();
        }
    }
}
