using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.UserType
{
    public class UpdateUserTypeDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
