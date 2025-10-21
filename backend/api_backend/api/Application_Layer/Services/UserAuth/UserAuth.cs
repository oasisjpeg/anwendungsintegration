using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

using System.Text;

namespace WebApplication1.Application_Layer.Services.UserAuth
{
    public class UserAuth : IUserAuth
    {
        private readonly PasswordHasher<object> _passwordHasher = new PasswordHasher<object>();
        public string HashPassword(string password)
        {
            var dummyUser = new UserAuth(); // or your actual user class
            return _passwordHasher.HashPassword(dummyUser, password);
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var dummyUser = new UserAuth();
            return _passwordHasher.VerifyHashedPassword(dummyUser, hashedPassword, providedPassword) 
                == PasswordVerificationResult.Success;
        }

        public Guid GetUserIdGuidFromClaims(string userIdString)
        {
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userIdGuid))
            {
                throw new UnauthorizedAccessException("Invalid or missing user ID");
            }
            return userIdGuid;
        }
    }
}
