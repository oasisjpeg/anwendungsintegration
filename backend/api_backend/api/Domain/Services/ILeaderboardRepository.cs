namespace WebApplication1.Domain.Services
{
    public interface ILeaderboardRepository
    {
        public Task<List<(string userName, int Score)>> GetLeaderboardForOneUser(Guid userId, int currentUsersScore);
        public Task<int> RecentPointIncreaseCurrentUser(Guid userId);
    }
}
