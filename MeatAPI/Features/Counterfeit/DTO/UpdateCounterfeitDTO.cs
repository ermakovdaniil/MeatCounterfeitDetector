﻿using System.ComponentModel.DataAnnotations;

namespace MeatAPI.Features.Counterfeit.DTO
{
    public class UpdateCounterfeitDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
