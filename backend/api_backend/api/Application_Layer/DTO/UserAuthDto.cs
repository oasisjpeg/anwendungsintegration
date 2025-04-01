using System.ComponentModel.DataAnnotations;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Application_Layer.DTO
{
    public class UserAuthDto
    {
        [Required]
        [MaxLength(100)]
        public required string Email { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(12)]
        public required string Password { get; set; }
    }
}
