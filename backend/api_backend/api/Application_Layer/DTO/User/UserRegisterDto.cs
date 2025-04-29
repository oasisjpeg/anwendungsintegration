using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Application_Layer.DTO.User
{
    public class UserRegisterDto
    {

        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public required string Email { get; set; }

        [Required]
        [MinLength(12)]
        [MaxLength(100)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*#?&]+$",
            ErrorMessage = "Password must contain at least one letter and one number.")]
        public required string Password { get; set; }

    }
}