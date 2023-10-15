﻿using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.Result
{
    public class CreateResultDTO
    {
        [Required(ErrorMessage = "Date is required")]
        public string Date { get; set; }

        [Required(ErrorMessage = "ResultId is required")]
        public Guid ResultId { get; set; }

        [Required(ErrorMessage = "AnRes is required")]
        public string AnRes { get; set; }

        [Required(ErrorMessage = "Time is required")]
        public double Time { get; set; }

        [Required(ErrorMessage = "PercentOfSimilarity is required")]
        public double PercentOfSimilarity { get; set; }

        [Required(ErrorMessage = "OrigPathId is required")]
        public Guid OrigPathId { get; set; }

        [Required(ErrorMessage = "ResPathId is required")]
        public Guid ResPathId { get; set; }
    }
}