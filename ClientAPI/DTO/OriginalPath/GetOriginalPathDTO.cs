﻿using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.OriginalPath
{
    public class GetOriginalPathDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Path { get; set; }
    }
}