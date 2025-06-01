using WebApplication1.Application_Layer.DTO.User;
using WebApplication1.Domain.Models.User;

namespace WebApplication1.Domain.Repositories;

public interface IUserServices
{
    Task<UserModel?> GetByEmailAsync(string email);
    Task<UserModel?> GetByIdAsync(string Id);
    // TODO: change to use Model objects instead of DTOs
    Task<UserModel> RegisterAsync(UserRegisterDto userRegisterDto);
    Task<UserModel> DeleteAsync(UserAuthDto userAuthDto);
    Task<UserModel> PatchAsync(UserPatchDto userPatchDto);
}