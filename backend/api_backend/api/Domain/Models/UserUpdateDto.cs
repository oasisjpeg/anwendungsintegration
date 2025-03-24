using WebApplication1.Domain.Repositories;

namespace WebApplication1.Domain.Models
{
    public class UserUpdateDto : IUserUpdateDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string CurrentPasswordHash { get; set; } // must never be null --> required for authetication & checked in cotroller
        public string? NewPasswordHash { get; set; }
    }
}
