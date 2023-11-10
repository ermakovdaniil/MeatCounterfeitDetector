using ClientAPI.DTO.UserType;
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

        //[Required(ErrorMessage = "TypeId is required")]
        //public Guid TypeId { get; set; }

        public string UserTypeName { get; set; }
    }
}