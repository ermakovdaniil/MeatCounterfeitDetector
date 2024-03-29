﻿using ClientAPI.DTO.Counterfeit;
using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.CounterfeitImage
{
    public class CreateCounterfeitImageDTO
    {
        [Required(ErrorMessage = "CounterfeitId is required")]
        public Guid CounterfeitId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string ImagePath { get; set; }

        public string CounterfeitName { get; set; }
    }
}