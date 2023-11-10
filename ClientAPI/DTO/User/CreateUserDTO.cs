using ClientAPI.DTO.UserRole;
using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.User
{
    public class CreateUserDTO
    {
        [Required(ErrorMessage = "Login is required")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string RoleName { get; set; }
    }
}