﻿using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.OriginalImage
{
    public class CreateOriginalImageDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string ImagePath { get; set; }
    }
}