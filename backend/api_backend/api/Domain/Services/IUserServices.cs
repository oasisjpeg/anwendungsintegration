using WebApplication1.Domain.Models.User;

namespace WebApplication1.Domain.Repositories;

public interface IUserServices
{
    Task<UserModel?> GetByEmailAsync(string email);
    Task<UserModel?> GetByIdAsync(string Id);
    Task<UserModel> RegisterAsync(string name, string email, string hashedPassword);
    Task<UserModel> DeleteAsync(string email, string password);
    Task<UserModel> PatchAsync(Guid userId, string? newName, string? newEmail, string? hashedNewPassword);
}