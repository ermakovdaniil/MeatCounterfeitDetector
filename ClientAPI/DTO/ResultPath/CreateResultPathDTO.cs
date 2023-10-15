﻿using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.ResultPath
{
    public class CreateResultPathDTO
    {
        [Required(ErrorMessage = "InitId is required")]
        public Guid InitId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Path { get; set; }
    }
}