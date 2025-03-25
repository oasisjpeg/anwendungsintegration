using WebApplication1.Domain.Repositories;

namespace WebApplication1.Domain.Models
{
    public interface UserAuthDto : IUserManagementDto
    {
        public string Id { get; set; }
        public string? PasswordHash { get; set; }
    }
}
