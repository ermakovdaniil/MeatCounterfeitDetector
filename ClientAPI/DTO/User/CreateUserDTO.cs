﻿using System.ComponentModel.DataAnnotations;

namespace ClientAPI.DTO.User
{
    public class CreateUserDTO
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "RoleName is required")]
        public List<string> Roles { get; set; }
    }
}