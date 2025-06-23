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
            if (points > 150)
            {
                points = 150;
            }

            return points;
        }

        public async Task CreateTransaction(PointSourceType pointSourceType, int pointSourceId, Guid userId, int pointAmount)
        {
            var newTransaction = new RewardTransactionModel
            {
                id = 0,
                Created = DateTime.Now,
                PointsGained = pointAmount,
                PointSourceType = pointSourceType,
                PointSourceId = pointSourceId,
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

        public async Task<bool> PreventDuplicateTransaction(Guid userId, PointSourceType sourceType, int articleId)
        {
            bool alreadyExists;
            switch (sourceType)
            {
                case PointSourceType.Article:
                    alreadyExists = await _dbContext.RewardTransactions
                        .AnyAsync(t => t.UserId == userId
                                       && t.PointSourceType == PointSourceType.Article
                                       && t.PointSourceId == articleId);
                    break;

                case PointSourceType.Quiz:
                    var quizId = await _dbContext.Quiz
                        .Where(q => q.ArticleId == articleId)
                        .Select(q => q.id)
                        .FirstOrDefaultAsync();

                    if (quizId == 0)
                        return false; // No quiz exists for this article, treat as not yet rewarded

                    return await _dbContext.RewardTransactions
                        .AnyAsync(t => t.UserId == userId
                                    && t.PointSourceType == PointSourceType.Quiz
                                    && t.PointSourceId == quizId);

                default:
                    throw new ArgumentException("Unsupported PointSourceType");
            }
            return alreadyExists;
        }
    }
}
