using System.ComponentModel.DataAnnotations;

namespace MeatAPI.Features.ResultPath.DTO
{
    public class DeleteResultPathDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "InitId is required")]
        public Guid InitId { get; set; }
    }
}
