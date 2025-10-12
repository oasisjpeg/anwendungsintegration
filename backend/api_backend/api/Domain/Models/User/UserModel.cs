using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Domain.Models.User
{
    public class UserModel : IdentityUser<Guid> // <-- Guid must be specified, otherwise string is default
    {
        public required string Name { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public required int Points { get; set; } = 0;
    }
}
