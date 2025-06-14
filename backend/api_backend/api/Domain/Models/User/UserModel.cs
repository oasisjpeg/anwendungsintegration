using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Domain.Models.User
{
    public class UserModel : IdentityUser<Guid> // <-- Guid must be specified, otherwise string is default
    {

        // removed Id because it is included in IdentityUser
        // public required string Id { get; set; }
        public required string Name { get; set; }

        // removed Email because it is included in IdentityUser
        // public required string Email { get; set; }

        // removed PasswordHash because it is included in IdentityUser
        //public required string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public required int Points { get; set; } = 0;
        
        // Refresh Token fields
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
