﻿using ClientAPI.DTO.OriginalImage;
using ClientAPI.DTO.Result;
using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.ResultImage
{
    public class CreateResultImageDTO
    {
        [Required(ErrorMessage = "OriginalId is required")]
        public Guid OriginalImageId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string ImagePath { get; set; }
    }
}