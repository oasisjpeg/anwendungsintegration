using WebApplication1.Domain.Models;

namespace WebApplication1.Domain.Repositories;

public interface IUserRepository
{
    Task<UserModel?> GetByEmailAsync(string email);
    Task<UserModel> GetByUserId(int userId);
    Task<UserModel> RegisterAsync(UserModel userModel);
}