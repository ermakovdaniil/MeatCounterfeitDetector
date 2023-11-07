using System.ComponentModel.DataAnnotations;
using ClientAPI.DTO.Counterfeit;

namespace ClientAPI.DTO.CounterfeitPath
{
    public class GetCounterfeitPathDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "CounterfeitId is required")]
        public Guid CounterfeitId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string ImagePath { get; set; } // TODO: возврат картинки в формате base-64 строки

        public string CounterfeitName { get; set; }
    }
}
