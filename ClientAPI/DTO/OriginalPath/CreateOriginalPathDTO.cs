using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.OriginalPath
{
    public class CreateOriginalPathDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Path { get; set; }
    }
}