using System.ComponentModel.DataAnnotations;

namespace MeatAPI.Features.OriginalPath.DTO
{
    public class CreateOriginalPathDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Path { get; set; }
    }
}
