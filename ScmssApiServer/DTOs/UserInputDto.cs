﻿using ScmssApiServer.Models;
using System.ComponentModel.DataAnnotations;
using ScmssApiServer.Validators;

namespace ScmssApiServer.DTOs
{
    public class UserInputDto
    {
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 5)]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        public required string Name { get; set; }

        [Required]
        public required Gender Gender { get; set; }

        [Required]
        public required DateTime DateOfBirth { get; set; }

        [Required]
        [IdCardNumber]
        public required string IdCardNumber { get; set; }

        public string? Address { get; set; }

        public string? Description { get; set; }
    }
}