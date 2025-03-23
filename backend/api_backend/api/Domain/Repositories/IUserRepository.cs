using WebApplication1.Domain.Models;

namespace WebApplication1.Domain.Repositories;

public interface IUserRepository
{
    Task<UserModel?> GetByEmailAsync(string email);
    Task<UserModel?> GetByIdAsync(string Id);
    Task<UserModel> RegisterAsync(UserModel userModel);
    Task<UserModel> DeleteAsync(UserModel userModel);
    Task<UserModel> PatchAsync(string Id, UserUpdateDto updateDto);
}