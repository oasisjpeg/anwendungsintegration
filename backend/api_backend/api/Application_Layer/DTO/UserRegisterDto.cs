using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Application_Layer.DTO
{
    public class UserRegisterDto
    {

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public required string Email { get; set; }

        [Required]
        [MinLength(12)]
        [MaxLength(100)]
        public required string Password { get; set; }

    }
}