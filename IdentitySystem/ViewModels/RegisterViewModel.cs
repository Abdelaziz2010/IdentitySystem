﻿using IdentitySystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace IdentitySystem.ViewModels
{
    public class RegisterViewModel
    {
        
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [ValidateDomainName(allowedDomainName:"gmail.com",ErrorMessage = "Domain Name Must be gmail.com")]
        [Remote(action:"IsEmailExist" , controller:"Account")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Country { get; set; }

    }
}
