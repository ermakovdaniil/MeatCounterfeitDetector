using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.Result
{
    public class GetResultDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public string Date { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "AnalysisResult is required")]
        public string AnalysisResult { get; set; }

        [Required(ErrorMessage = "Time is required")]
        public double Time { get; set; }

        [Required(ErrorMessage = "PercentOfSimilarity is required")]
        public double PercentOfSimilarity { get; set; }

        [Required(ErrorMessage = "OriginalImageId is required")]
        public Guid OriginalImageId { get; set; }

        [Required(ErrorMessage = "ResultImageId is required")]
        public Guid? ResultImageId { get; set; }

        public string UserName { get; set; }
        public string OriginalEncodedImage { get; set; }
        public string? ResultEncodedImage { get; set; }
    }
}
