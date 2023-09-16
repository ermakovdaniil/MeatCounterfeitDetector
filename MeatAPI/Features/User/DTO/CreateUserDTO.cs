using DataAccess.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MeatAPI.Features.User.DTO
{
    public class CreateUserDTO
    {
        [Required(ErrorMessage = "Login is required")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "TypeId is required")]
        public Guid TypeId { get; set; }
    }
}