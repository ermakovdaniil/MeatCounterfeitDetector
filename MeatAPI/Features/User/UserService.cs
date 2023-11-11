using ClientAPI.DTO.User;
using DataAccess.Data;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MeatAPI.Features.User
{
    public class UserService : EntityAccessServiceBase<ResultDBContext, DataAccess.Models.User>
    {
        private readonly UserManager<DataAccess.Models.User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public UserService(UserManager<DataAccess.Models.User> userManager, RoleManager<IdentityRole<Guid>> roleManager, ResultDBContext dbContext) : base(dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<GetUserDTO>> GetAll()
        {
            var users = await _userManager.Users.ToListAsync();
            var usersDTO = new List<GetUserDTO>();

            foreach (var u in users)
            {
                var dto = new GetUserDTO();
                dto.Id = u.Id;
                dto.UserName = u.UserName;
                dto.Password = u.PasswordHash;
                dto.Name = u.Name;
                dto.Roles = (await _userManager.GetRolesAsync(u)).ToList();
                usersDTO.Add(dto);
            }

            return usersDTO;
        }

        public async Task<Guid> Create(CreateUserDTO u)
        {
            DataAccess.Models.User user = new()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = u.UserName,
                Name = u.Name,
            };

            await _userManager.CreateAsync(user, u.Password);
            await _userManager.AddToRolesAsync(user, u.Roles);

            //await _dbSet.AddAsync(user);
            //await _dbContext.SaveChangesAsync();

            return user.Id;
        }

        public async Task Update(UpdateUserDTO u)
        {
            var ut = await _userManager.FindByIdAsync(u.Id.ToString());

            ut.UserName = u.Name;
        
            if(u.Password is not null)
            {
                var passwordHasher = new PasswordHasher<DataAccess.Models.User>();
                ut.PasswordHash = passwordHasher.HashPassword(ut, u.Password);

                // ИЛИ

                await _userManager.RemovePasswordAsync(ut);
                await _userManager.AddPasswordAsync(ut, u.Password);
            }

            ut.Name = u.Name;
            
            await _userManager.UpdateAsync(ut);
            await _userManager.AddToRolesAsync(ut, u.Roles);

            //await _dbSet.AddAsync(user);
            //await _dbContext.SaveChangesAsync();
        }
    }
}