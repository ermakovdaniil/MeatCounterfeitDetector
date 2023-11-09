using ClientAPI.DTO.CounterfeitImage;
using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.Counterfeit
{
    public class CreateCounterfeitDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
