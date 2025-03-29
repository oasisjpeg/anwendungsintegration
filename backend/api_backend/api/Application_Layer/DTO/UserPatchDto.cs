using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Application_Layer.DTO
{
    public class UserPatchDto
    {

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public required string Email { get; set; }

        [Required]
        [MinLength(12)]
        [MaxLength(100)]
        public required string Password { get; set; }

        [MaxLength(100)]
        public string? NewName { get; set; }

        [MaxLength(100)]
        public string? NewPassword { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? NewEmail { get; set; }

    }
}
