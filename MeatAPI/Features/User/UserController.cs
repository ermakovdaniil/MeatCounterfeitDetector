using ClientAPI.DTO.User;
using DataAccess.Data;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.User
{
    public class UserController : BaseAuthorizedController
    {
        private readonly EntityAccessServiceBase<ResultDBContext, DataAccess.Models.User> _userService;

        public UserController(EntityAccessServiceBase<ResultDBContext, DataAccess.Models.User> userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<GetUserDTO>>> GetAll()
        {
            var users = await _userService.GetAll();
            var usersDTO = users.Adapt<List<GetUserDTO>>();

            //foreach (var u in users)
            //{
                //var dto = new GetUserDTO();
                //dto.Id = u.Id;
                //dto.Login = u.Login;
                //dto.Password = u.Password;
                //dto.Name = u.Name;
                //dto.UserRoleName = u.UserRole.Name;
                //usersDTO.Add(dto);
            //}

            foreach (var u in users)
            {
                foreach (var r in u.IdentityRoles)
                {
                    var dto = new GetUserDTO();
                    dto.Id = u.Id;
                    dto.Login = u.Login;
                    dto.Password = u.Password;
                    dto.Name = u.Name;
                    dto.RoleName = r.Name;
                    usersDTO.Add(dto);
                }
            }

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
