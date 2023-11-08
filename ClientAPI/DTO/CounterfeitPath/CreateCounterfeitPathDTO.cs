using ClientAPI.DTO.Counterfeit;
using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.CounterfeitPath
{
    public class CreateCounterfeitPathDTO
    {
        //[Required(ErrorMessage = "CounterfeitId is required")]
        //public Guid CounterfeitId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string EncodedImage { get; set; }

        public CreateCounterfeitDTO Counterfeit { get; set; }
    }
}