using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.ResultImage
{
    public class GetResultImageDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "OriginalId is required")]
        public Guid OriginalImageId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string ImagePath { get; set; }
    }
}
