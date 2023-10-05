using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.UserType
{
    public class CreateUserTypeDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
