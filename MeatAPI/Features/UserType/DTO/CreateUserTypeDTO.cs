using System.ComponentModel.DataAnnotations;

namespace MeatAPI.Features.UserType.DTO
{
    public class CreateUserTypeDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
