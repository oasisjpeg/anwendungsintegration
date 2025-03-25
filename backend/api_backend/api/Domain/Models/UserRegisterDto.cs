using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Domain.Models
{
    public class UserRegisterDto : IUserManagementDto
    {
        // What is the difference between [Required] and required?
        [Required] // note to self: --> Ensures API request contains the value
        [Key]      
        public required string Id { get; set; } // note to self: --> this "required" Ensures it's initialized at compile-time

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string CurrentPasswordHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public bool VerifyPassword(string password)
        {
            return HashPassword(password) == this.CurrentPasswordHash;
        }
    }
}