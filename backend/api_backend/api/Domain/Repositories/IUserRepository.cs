using WebApplication1.Application_Layer.DTO;
using WebApplication1.Domain.Models.User;

namespace WebApplication1.Domain.Repositories;

public interface IUserRepository
{
    Task<UserModel?> GetByEmailAsync(string email);
    Task<UserModel?> GetByIdAsync(string Id);
    Task<UserModel> RegisterAsync(UserRegisterDto userRegisterDto);
    Task<UserModel> DeleteAsync(UserAuthDto userAuthDto);
    Task<UserModel> PatchAsync(string userId, UserPatchDto dto);
}