using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Models.User;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Infrastructure.MySqlRepositories
{
    public class MySqlTransactionRepository : ITransactionRepository
    {
        private readonly MySqlDbContext _dbContext;
        public MySqlTransactionRepository(MySqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> GetArticlePoints(int? articleId)
        {
            var articleContent = await _dbContext.Articles
                .Where(a => a.id == articleId)
                .Select(a => a.Content)
                .FirstOrDefaultAsync();

            if (articleContent == null)
            {
                throw new DirectoryNotFoundException();
            }

            var wordCount = articleContent.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

            var points = (int)Math.Round(0.2 * wordCount);
            if (points > 300)
            {
                points = 300;
            }

            return points;
        }

        public async Task CreateTransaction(PointSourceType pointSourceType, Guid userId, int pointAmount)
        {
            var newTransaction = new RewardTransactionModel
            {
                id = 0,
                Created = DateTime.Now,
                PointsGained = pointAmount,
                PointSourceType = pointSourceType,
                UserId = userId
            };

            _dbContext.RewardTransactions.Add(newTransaction);

            // Update the user's points
            var user = await _dbContext.Users.FindAsync(userId);
            if (user != null)
            {
                user.Points += pointAmount;
                _dbContext.Users.Update(user); // Not strictly needed if user is tracked, but safe to include
            }

            await _dbContext.SaveChangesAsync();
        }

    }
}
