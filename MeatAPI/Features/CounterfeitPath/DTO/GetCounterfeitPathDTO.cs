using System.ComponentModel.DataAnnotations;

namespace MeatAPI.Features.CounterfeitPath.DTO
{
    public class GetCounterfeitPathDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "CounterfeitId is required")]
        public Guid CounterfeitId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string ImagePath { get; set; }
    }
}
