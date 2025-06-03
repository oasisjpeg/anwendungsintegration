using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Models.Consumption;
using WebApplication1.Domain.Models.User;
using WebApplication1.Domain.Models.Article;
using WebApplication1.Domain.Repositories;
using WebApplication1.Application_Layer.DTO.User;


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
        if (!new EmailAddressAttribute().IsValid(email))
        {
            return null; // Invalid email format
        }
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<UserModel?> GetByIdAsync(Guid Id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
    }

    public async Task<UserModel> RegisterAsync(string name, string email, string passwordHash)
    {
        var userModel = new UserModel
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Points = 0
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
                id = Guid.NewGuid().ToString(),
                UserId = userModel.Id,
                Timestamp = baseDate.AddHours(i),
                KWValue = kWValuesConsumption[i]
            });
        }

        // Create recommendation records (inverse pattern)
        var dummyRecommendedRecords = new List<RecommendRecordModel>();
        for (int i = 0; i < 24; i++)
        {
            dummyRecommendedRecords.Add(new RecommendRecordModel()
            {
                id = Guid.NewGuid().ToString(),
                UserId = userModel.Id,
                Created = baseDate.AddHours(i),
                KWValue = kWValuesRecommended[i]
            });
        }


        _context.ConsumptionRecords.AddRange(dummyConsumptionRecords);
        _context.RecommendRecords.AddRange(dummyRecommendedRecords);

        await _context.SaveChangesAsync(); // Save the dummy data
        
        return userModel;
    }

    public async Task<UserModel> DeleteAsync(string email)
    {
        var user = await GetByEmailAsync(email);
        if (user == null)
            throw new Exception("User not found.");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<UserModel> PatchAsync(Guid userId, string? newName, string? newEmail, string? newPasswordHash)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
            throw new Exception("User not found.");

        if (!string.IsNullOrWhiteSpace(newName))
        {
            user.Name = newName;
        }

        if (!string.IsNullOrWhiteSpace(newEmail))
        {
            user.Email = newEmail;
            user.UserName = newEmail; // if using Identity
        }

        if (!string.IsNullOrWhiteSpace(newPasswordHash))
        {
            user.PasswordHash = newPasswordHash;
        }

        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return user;
    }


    public async Task<UserModel?> GetByEmailAndPasswordAsync(string email, string passwordHash)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == passwordHash);
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

    public async Task<List<RewardTransactionModel>> GetRecentTransactionsAsync(Guid userId, int count)
    {
        return await _context.RewardTransactions
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Created) 
            .Take(count)
            .ToListAsync();
    }
}