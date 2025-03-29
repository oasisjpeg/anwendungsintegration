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
            return _passwordHasher.HashPassword(null, password);
        }

        public bool VerifyPassword(string hashedPassword ,string password)
        {
            return _passwordHasher.VerifyHashedPassword(null, hashedPassword, password) 
                == PasswordVerificationResult.Success;
        }
    }
}
