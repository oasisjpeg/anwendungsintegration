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

    public async Task<UserModel?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<UserModel?> GetByIdAsync(string Id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
    }

    public async Task<UserModel> RegisterAsync(UserModel userModel)
    {
        _context.Users.Add(userModel);
        await _context.SaveChangesAsync();
        return userModel;
    }

    public async Task<UserModel> DeleteAsync(UserModel userModel)
    {
        _context.Users.Remove(userModel);
        await _context.SaveChangesAsync();
        return userModel;
    }

    public async Task<UserModel> PatchAsync(string Id, UserUpdateDto updateDto)
    {
        var user = await GetByIdAsync(Id); // can never be null, as the controller checks this

        if (!string.IsNullOrEmpty(updateDto.Name))
        {
            user.Name = updateDto.Name;
        }
        if (!string.IsNullOrEmpty(updateDto.Email))
        {
            user.Email = updateDto.Email;
        }
        if (!string.IsNullOrEmpty(updateDto.NewPasswordHash))
        {
            user.PasswordHash = UserModel.HashPassword(updateDto.NewPasswordHash);
        }
        await _context.SaveChangesAsync();
        return user;
    }
}