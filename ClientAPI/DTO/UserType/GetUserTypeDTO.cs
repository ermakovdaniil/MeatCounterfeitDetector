using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.UserType
{
    public class GetUserTypeDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
