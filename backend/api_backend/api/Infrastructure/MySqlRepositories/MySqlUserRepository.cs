using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Models.User;
using WebApplication1.Domain.Repositories;
using WebApplication1.Application_Layer.DTO.User;
using WebApplication1.Application_Layer.Services.ConsumptionData;


namespace WebApplication1.Infrastructure.MySqlRepositories;

public class MySqlUserRepository : IUserRepository
{
    private readonly MySqlDbContext _context;
    private readonly IConsumptionDataService _consumptionDataService;

    public MySqlUserRepository(MySqlDbContext context, IConsumptionDataService consumptionDataService)
    {
        _context = context;
        _consumptionDataService = consumptionDataService;
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
        //AsNoTrackig is for workbench changes
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == Id);
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
        
        // Generate initial consumption and recommendation data
        var consumptionRecords = await _consumptionDataService.GenerateInitialConsumptionDataAsync(userModel.Id);
        var recommendationRecords = await _consumptionDataService.GenerateInitialRecommendationDataAsync(userModel.Id);

        _context.ConsumptionRecords.AddRange(consumptionRecords);
        _context.RecommendRecords.AddRange(recommendationRecords);

        await _context.SaveChangesAsync();
        
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
    

    public async Task<List<RewardTransactionModel>> GetRecentTransactionsAsync(Guid userId, int count)
    {
        // 1. Get transactions without names
        var transactions = await _context.RewardTransactions
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Created)
            .Take(count)
            .ToListAsync();

    
        var articleIds = transactions
            .Where(t => t.PointSourceType == PointSourceType.Article)
            .Select(t => t.PointSourceId)
            .Distinct().ToList();
    
        var quizIds = transactions
            .Where(t => t.PointSourceType == PointSourceType.Quiz)
            .Select(t => t.PointSourceId)
            .Distinct().ToList();

        var articleNames = await _context.Articles
            .Where(a => articleIds.Contains(a.id))
            .ToDictionaryAsync(a => a.id, a => a.Title);
    
        var quizNames = await _context.Quiz
            .Where(q => quizIds.Contains(q.id))
            .ToDictionaryAsync(q => q.id, q => q.Title);

        foreach (var t in transactions)
        {
            if (t.PointSourceType == PointSourceType.Article 
                && articleNames.TryGetValue(t.PointSourceId, out var articleName))
            {
                t.SourceName = articleName;
            }
            else if (t.PointSourceType == PointSourceType.Quiz 
                     && quizNames.TryGetValue(t.PointSourceId, out var quizName))
            {
                t.SourceName = quizName;
            }
        }

        return transactions;
    }

}