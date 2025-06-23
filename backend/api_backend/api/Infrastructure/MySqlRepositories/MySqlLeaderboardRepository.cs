using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Services;

namespace WebApplication1.Infrastructure.MySqlRepositories
{
    public class MySqlLeaderboardRepository : ILeaderboardRepository
    {
        private MySqlDbContext _dbContext;
        public MySqlLeaderboardRepository(MySqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<(string userName, int Score)>> GetLeaderboardForOneUser(Guid userId, int currentUsersScore) // <-- does not scale well --> eventually think of better solution for the leaderboard
        {
            var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);

            var recentTransactions = await _dbContext.RewardTransactions
                .Where(t => t.Created >= sevenDaysAgo && t.PointsGained > 0)
                .ToListAsync();

            var userScores = recentTransactions
                .GroupBy(t => t.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    Score = g.Sum(t => t.PointsGained)
                })
                .ToList();

            var aboveUsers = userScores
                .Where(u => u.UserId != userId && u.Score > currentUsersScore)
                .OrderBy(u => u.Score)
                .Take(5)
                .ToList();

            var belowUsers = userScores
                .Where(u => u.UserId != userId && u.Score < currentUsersScore)
                .OrderByDescending(u => u.Score)
                .Take(5)
                .ToList();

            var comparisonUserIds = aboveUsers.Concat(belowUsers).Select(u => u.UserId).ToList();
            var userInfos = await _dbContext.Users
                .Where(u => comparisonUserIds.Contains(u.Id))
                .Select(u => new { u.Id, u.Name })
                .ToListAsync();

            var result = aboveUsers
                .Concat(belowUsers)
                .Select(u =>
                {
                    var userName = userInfos.FirstOrDefault(info => info.Id == u.UserId)?.Name ?? "Unknown";
                    return (userName, u.Score);
                })
                .ToList();

            return result;
        }

        public async Task<int> RecentPointIncreaseCurrentUser(Guid userId) 
        {
            var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);

            var pointIncrease = await _dbContext.RewardTransactions
                .Where(t => t.UserId == userId && t.PointsGained > 0 && t.Created >= sevenDaysAgo)
                .SumAsync(t => t.PointsGained);


            //if (pointIncrease == null)
            //{
            //    throw new Exception("Point increase is null for this user.");
            //}

            return pointIncrease; // returns point INCREASE over last 7 days for current user
        }
    }
}
