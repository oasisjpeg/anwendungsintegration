using System.ComponentModel.DataAnnotations;
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
    
        // Generate duck curve consumption pattern
        var (kWValuesConsumption, kWValuesRecommended) = GenerateDuckCurveData();

        // Create consumption records
        var dummyConsumptionRecords = new List<ConsumptionRecordModel>();
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

        // Create recommendation records (inverse pattern)
        var dummyRecommendedRecords = new List<RecommendRecordModel>();
        for (int i = 0; i < 24; i++)
        {
            dummyRecommendedRecords.Add(new RecommendRecordModel()
            {
                RecommendId = Guid.NewGuid().ToString(),
                UserId = userModel.Id,
                Timestamp = baseDate.AddHours(i),
                kWValue = kWValuesRecommended[i]
            });
        }


        _context.ConsumptionRecords.AddRange(dummyConsumptionRecords);
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
    
    private (double[] consumption, double[] recommended) GenerateDuckCurveData()
    {
        // Duck curve parameters (adjusted from search results)
        var hours = Enumerable.Range(0, 24).Select(h => (double)h).ToArray();
    
        // Baseline load pattern
        var baseline = hours.Select(h => 
                1.2 + 0.8 * Math.Sin((h - 6) / 24 * 2 * Math.PI) // Morning/evening peaks
        ).ToArray();

        // Solar generation dip
        var solarDip = hours.Select(h => 
                1.5 * Math.Exp(-0.5 * Math.Pow((h - 13) / 2.5, 2)) // Peak at 13:00
        ).ToArray();

        // Net consumption (baseline + evening peak - solar generation)
        var netConsumption = hours.Select((h, i) => 
            baseline[i] + 0.5 * Math.Exp(-0.5 * Math.Pow((h - 19)/2, 2)) - solarDip[i]
        ).ToArray();

        // Normalize to match Excel data range (3-13 kWh daily total)
        var minConsumption = netConsumption.Min();
        var maxConsumption = netConsumption.Max();
        var scaledConsumption = netConsumption.Select(c => 
            0.2 + (c - minConsumption) * (1.8 - 0.2) / (maxConsumption - minConsumption)
        ).ToArray();

        // Create recommendation curve (encourage shifting to solar hours)
        var maxRec = scaledConsumption.Max();
        var recommended = scaledConsumption.Select(c => 
            Math.Round(maxRec - c + 0.3, 1)
        ).ToArray();

        return (scaledConsumption, recommended);
    }
}