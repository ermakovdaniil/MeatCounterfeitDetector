using ClientAPI.DTO.User;
using ClientAPI.DTO.UserRole;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MeatAPI.Features.UserRole
{
    public class UserRoleService
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public UserRoleService(RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<GetUserRoleDTO>> GetAll()
        {
            var userRoles = await _roleManager.Roles.ToListAsync();
            var userRolesDTO = new List<GetUserRoleDTO>();

            foreach (var u in userRoles)
            {
                var dto = new GetUserRoleDTO();
                dto.Id = u.Id;
                dto.Name = u.Name;
                userRolesDTO.Add(dto);
            }
            return userRolesDTO;
        }
    }
}