using WebApplication1.Domain.Models.User;

namespace WebApplication1.Domain.Repositories;

public interface IUserRepository
{
    Task<UserModel?> GetByEmailAsync(string email);
    Task<UserModel?> GetByIdAsync(Guid Id);
    Task<UserModel> RegisterAsync(string name, string email, string passwordHash);
    Task<UserModel> DeleteAsync(string email);
    Task<UserModel> PatchAsync(Guid userId, string? newName, string? newEmail, string? newPasswordHash);
    Task<UserModel?> GetByEmailAndPasswordAsync(string email, string passwordHash);
    Task<List<RewardTransactionModel>> GetRecentTransactionsAsync(Guid userId, int count);
}