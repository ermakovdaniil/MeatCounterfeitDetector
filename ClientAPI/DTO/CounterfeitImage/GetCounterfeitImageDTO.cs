using System.ComponentModel.DataAnnotations;
using ClientAPI.DTO.Counterfeit;

namespace ClientAPI.DTO.CounterfeitImage
{
    public class GetCounterfeitImageDTO
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
