using ClientAPI.DTO.Result;
using ClientAPI.DTO.ResultImage;
using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.OriginalImage
{
    public class GetOriginalImageDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string EncodedImage { get; set; }
    }
}
