using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.OriginalPath
{
    public class UpdateOriginalPathDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Path { get; set; }
    }
}
