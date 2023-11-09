using ClientAPI.DTO.OriginalImage;
using ClientAPI.DTO.Result;
using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.ResultImage
{
    public class CreateResultImageDTO
    {
        //[Required(ErrorMessage = "OriginalId is required")]
        //public Guid OriginalId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string EncodedImage { get; set; }

        public CreateOriginalImageDTO OriginalImage { get; set; }
    }
}