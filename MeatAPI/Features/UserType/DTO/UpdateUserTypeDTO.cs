using System.ComponentModel.DataAnnotations;

namespace MeatAPI.Features.UserType.DTO
{
    public class UpdateUserTypeDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
