
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Application_Layer.Services.UserExistCheck
{
    public class UserExistCheck : IUserExistCheck
    {
        private readonly IUserRepository _userRepository;

        public UserExistCheck(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<bool> UserExistsAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null;
        }
    }
}
