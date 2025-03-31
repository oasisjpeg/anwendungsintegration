using Microsoft.EntityFrameworkCore;
using WebApplication1.Application_Layer.DTO;
using WebApplication1.Application_Layer.Services.UserAuth;
using WebApplication1.Domain.Models;
using WebApplication1.Domain.Repositories;


namespace WebApplication1.Infrastructure.MySqlRepositories;

public class MySqlUserRepository : IUserRepository
{
    private readonly MySqlDbContext _context;
    private readonly IUserAuth _userAuth;

    public MySqlUserRepository(MySqlDbContext context, IUserAuth userAuth)
    {
        _context = context;
        _userAuth = userAuth;
    }

    public async Task<UserModel?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<UserModel?> GetByIdAsync(string Id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
    }

    public async Task<UserModel> RegisterAsync(UserRegisterDto userRegisterDto)
    {
        var userModel = new UserModel
        {
            Id = Guid.NewGuid().ToString(),
            Name = userRegisterDto.Name,
            Email = userRegisterDto.Email,
            PasswordHash = _userAuth.HashPassword(userRegisterDto.Password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(userModel);
        userModel.CreatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return userModel;
    }

    public async Task<UserModel> DeleteAsync(UserAuthDto userAuthDto)
    {
        var user = await GetByEmailAsync(userAuthDto.Email);

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return (user);
    }

    public async Task<UserModel> PatchAsync(UserPatchDto userPatchDto)
    {
        var user = await GetByEmailAsync(userPatchDto.Email);

        if (!string.IsNullOrEmpty(userPatchDto.NewName))
        {
            user.Name = userPatchDto.NewName;
        }
        if (!string.IsNullOrEmpty(userPatchDto.NewEmail))
        {
            user.Email = userPatchDto.Email;
        }
        if (!string.IsNullOrEmpty(userPatchDto.NewPassword))
        {
            user.PasswordHash = _userAuth.HashPassword(userPatchDto.NewPassword);
        }

        // updateAt timestamp to track last update time
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<UserModel?> GetInformationFromUserAsync(UserAuthDto userAuthDto)
    {
        var user = await GetByEmailAsync(userAuthDto.Email);

        if (user == null)
        {
            return null; // Benutzer nicht gefunden
        }

        if (!_userAuth.VerifyPassword(user.CurrentPasswordHash, userAuthDto.Password))
        {
            return null; // Ungültiges Passwort
        }

        await _context.SaveChangesAsync();
        return user;
    }
}