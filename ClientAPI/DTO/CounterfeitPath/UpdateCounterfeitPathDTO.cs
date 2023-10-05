using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.CounterfeitPath
{
    public class UpdateCounterfeitPathDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "CounterfeitId is required")]
        public Guid CounterfeitId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string ImagePath { get; set; }
    }
}
