﻿using ClientAPI.DTO.OriginalPath;
using ClientAPI.DTO.User;
using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.Result
{
    public class GetResultDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public string Date { get; set; }

        //[Required(ErrorMessage = "UserId is required")]
        //public Guid UserId { get; set; }

        [Required(ErrorMessage = "AnRes is required")]
        public string AnRes { get; set; }

        [Required(ErrorMessage = "Time is required")]
        public double Time { get; set; }

        [Required(ErrorMessage = "PercentOfSimilarity is required")]
        public double PercentOfSimilarity { get; set; }

        //[Required(ErrorMessage = "OrigPathId is required")]
        //public Guid OrigPathId { get; set; }

        //[Required(ErrorMessage = "ResPathId is required")]
        //public Guid ResPathId { get; set; }

        public GetUserDTO User { get; set; }
        public GetOriginalPathDTO OriginalPath { get; set; }
        public GetResultDTO Result { get; set; }
    }
}
