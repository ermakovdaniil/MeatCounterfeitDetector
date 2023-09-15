using System.ComponentModel.DataAnnotations;

namespace MeatAPI.Features.Counterfeit.DTO
{
    public class CreateCounterfeitDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
    }
}
