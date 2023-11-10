using ClientAPI.DTO.Counterfeit;
using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.CounterfeitImage
{
    public class UpdateCounterfeitImageDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "CounterfeitId is required")]
        public Guid CounterfeitId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string EncodedImage { get; set; }

        public string CounterfeitName { get; set; }
    }
}
