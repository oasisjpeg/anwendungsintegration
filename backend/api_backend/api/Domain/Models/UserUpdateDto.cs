using WebApplication1.Domain.Repositories;

namespace WebApplication1.Domain.Models
{
    public class UserUpdateDto : IUserUpdateDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string CurrentPasswordHash { get; set; }
        public string? NewPasswordHash { get; set; }
    }
}
