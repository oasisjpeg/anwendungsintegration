using WebApplication1.Domain.Models;

namespace WebApplication1.Domain.Repositories;

public interface IUserRepository
{
    Task<UserRegisterDto?> GetByEmailAsync(string email);
    Task<UserRegisterDto?> GetByIdAsync(string Id);
    Task<UserRegisterDto> RegisterAsync(UserRegisterDto userModel);
    Task<IUserManagementDto> DeleteAsync(UserAuthDto userModel);
    Task<IUserManagementDto> PatchAsync(UserPatchDto updateDto);
}