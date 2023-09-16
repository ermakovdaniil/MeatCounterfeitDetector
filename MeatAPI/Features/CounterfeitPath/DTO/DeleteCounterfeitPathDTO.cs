using System.ComponentModel.DataAnnotations;

namespace MeatAPI.Features.CounterfeitPath.DTO
{
    public class DeleteCounterfeitPathDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "CounterfeitId is required")]
        public Guid CounterfeitId { get; set; }
    }
}
