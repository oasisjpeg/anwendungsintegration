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

    public async Task<UserModel> RegisterAsync(UserModel userModel)
    {
        _context.Users.Add(userModel);
        await _context.SaveChangesAsync();
        return userModel;
    }
}