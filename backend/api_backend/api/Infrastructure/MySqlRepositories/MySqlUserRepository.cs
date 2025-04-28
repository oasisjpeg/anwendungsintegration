using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Application_Layer.DTO;
using WebApplication1.Application_Layer.Services.UserAuth;
using WebApplication1.Domain.Models.Consumption;
using WebApplication1.Domain.Models.User;
using WebApplication1.Domain.Models.Article;
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
        if (!new EmailAddressAttribute().IsValid(email))
        {
            return null; // Invalid email format
        }
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
            PasswordHash = userRegisterDto.Password,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(userModel);
        userModel.CreatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        
        var baseDate = DateTime.Parse("2025-03-13 00:00:00");
        var dummyConsumptionRecords = new List<ConsumptionRecordModel>();

        var kWValuesConsumption = new double[]
        {
            0.5, 0.8, 0.4, 0.3, 0.2, 0.4, 0.6, 1.2, 2.1, 1.8, 1.5, 1.3,
            2.5, 2.8, 3.2, 3.5, 3.0, 2.7, 2.4, 2.0, 1.8, 1.5, 1.0, 0.7
        };

        for (int i = 0; i < 24; i++)
        {
            dummyConsumptionRecords.Add(new ConsumptionRecordModel
            {
                ConsumptionId = Guid.NewGuid().ToString(),
                UserId = userModel.Id,
                Timestamp = baseDate.AddHours(i),
                kWValue = kWValuesConsumption[i]
            });
        }

        _context.ConsumptionRecords.AddRange(dummyConsumptionRecords);
        
        var dummyRecommendedRecords = new List<RecommendRecordModel>();

        var kWValuesRecommended = new double[]
        {
            0.3, 0.6, 0.3, 0.2, 0.1, 0.3, 0.4, 0.9, 1.6, 1.3, 1.1, 1.0,
            1.8, 2.0, 2.4, 2.6, 2.2, 2.0, 1.7, 1.5, 1.2, 1.0, 0.7, 0.5
        };

        for (int i = 0; i < 24; i++)
        {
            dummyRecommendedRecords.Add(new RecommendRecordModel()
            {
                RecommendId = Guid.NewGuid().ToString(),
                UserId = userModel.Id,
                Created = baseDate.AddHours(i),
                kWValue = kWValuesRecommended[i]
            });
        }

        _context.RecommendRecords.AddRange(dummyRecommendedRecords);

        await _context.SaveChangesAsync(); // Save the dummy data
        
        return userModel;
    }

    public async Task<UserModel> DeleteAsync(UserAuthDto userAuthDto)
    {
        var user = await GetByEmailAsync(userAuthDto.Email);

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return (user);
    }

    public async Task<UserModel> PatchAsync(string userId, UserPatchDto userPatchDto)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
            throw new Exception("User not found.");

        if (!string.IsNullOrWhiteSpace(userPatchDto.NewName))
        {
            user.Name = userPatchDto.NewName;
        }

        if (!string.IsNullOrWhiteSpace(userPatchDto.NewEmail))
        {
            user.Email = userPatchDto.NewEmail;
            user.UserName = userPatchDto.NewEmail; // if using Identity
        }

        if (!string.IsNullOrWhiteSpace(userPatchDto.NewPassword))
        {
            user.PasswordHash = _userAuth.HashPassword(userPatchDto.NewPassword);
        }

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

        if (!_userAuth.VerifyPassword(user.PasswordHash, userAuthDto.Password))
        {
            return null; // Ungültiges Passwort
        }

        await _context.SaveChangesAsync();
        return user;
    }
}