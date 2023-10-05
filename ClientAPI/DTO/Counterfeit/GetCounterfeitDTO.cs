using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.Counterfeit
{
    public class GetCounterfeitDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
