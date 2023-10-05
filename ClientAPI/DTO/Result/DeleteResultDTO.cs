using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.Result
{
    public class DeleteResultDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "OrigPathId is required")]
        public Guid OrigPathId { get; set; }

        [Required(ErrorMessage = "ResPathId is required")]
        public Guid ResPathId { get; set; }
    }
}
