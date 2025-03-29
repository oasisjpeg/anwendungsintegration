using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Domain.Models
{
    public class UserModel
    {
        [Key]
        public required string Id { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }

        public required string CurrentPasswordHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
