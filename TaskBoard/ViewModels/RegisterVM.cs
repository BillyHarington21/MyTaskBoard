﻿using System.ComponentModel.DataAnnotations;

namespace TaskBoard.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = " Password do not match.")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

    }
}
