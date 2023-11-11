using ClientAPI.DTO.Counterfeit;
using ClientAPI.DTO.User;
using ClientAPI.DTO.UserRole;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.UserRole
{
    public class UserRoleController : BaseAuthorizedController
    {
        private readonly UserRoleService _userRoleService;

        public UserRoleController(UserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<GetUserRoleDTO>>> GetAll()
        {
            var userRolesDTO = await _userRoleService.GetAll();
            return Ok(userRolesDTO);
        }
    }
}
