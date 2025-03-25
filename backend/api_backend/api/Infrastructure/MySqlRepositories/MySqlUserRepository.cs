using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Models;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Infrastructure.MySqlRepositories;

public class MySqlUserRepository : IUserRepository
{
    private readonly MySqlDbContext _context;

    public MySqlUserRepository(MySqlDbContext context)
    {
        _context = context;
    }

    public async Task<UserRegisterDto?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<UserRegisterDto?> GetByIdAsync(string Id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
    }

    public async Task<UserRegisterDto> RegisterAsync(UserRegisterDto userModel)
    {
        _context.Users.Add(userModel);
        await _context.SaveChangesAsync();
        return userModel;
    }
    public async Task<IUserManagementDto> DeleteAsync(UserAuthDto userModel)
    {
        _context.Users.Remove((UserRegisterDto)userModel);
        await _context.SaveChangesAsync();
        return userModel;
    }

    public async Task<IUserManagementDto> PatchAsync(UserPatchDto userAuthDto)
    {
        var user = await GetByIdAsync(userAuthDto.Id); // can never be null, as the controller checks this

        if (!string.IsNullOrEmpty(userAuthDto.Name))
        {
            user.Name = userAuthDto.Name;
        }
        if (!string.IsNullOrEmpty(userAuthDto.Email))
        {
            user.Email = userAuthDto.Email;
        }
        if (!string.IsNullOrEmpty(userAuthDto.NewPasswordHash))
        {
            user.CurrentPasswordHash = UserRegisterDto.HashPassword(userAuthDto.NewPasswordHash);
        }
        await _context.SaveChangesAsync();
        return user;
    }
}