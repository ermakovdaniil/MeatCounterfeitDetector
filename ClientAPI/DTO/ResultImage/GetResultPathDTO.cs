using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.ResultImage
{
    public class GetResultImageDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "OriginalId is required")]
        public Guid OriginalId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string EncodedImage { get; set; }

        //public string OriginalEncodedImage { get; set; }
    }
}
